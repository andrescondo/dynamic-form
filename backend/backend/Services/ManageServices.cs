using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using backend.Models.Response;
using backend.Models.Request;
using backend.Network;
using backend.Data;
using Newtonsoft.Json;
using System.Dynamic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Transactions;

namespace backend.Services
{
    public interface IManageService
	{
		Task<ResponseGeneral> GetAllFormAsync();
        Task<ResponseGeneral> GetFormAsync(int id, int audience);
        Task<ResponseGeneral> PostCreateForm(CreateTableRequest form);
        Task<ResponseGeneral> PostEditForm(EditTableRequest form);

    }
    public class ManageServices: IManageService
    {
        private readonly ApplicationDbContext _configuration;
        public ManageServices(ApplicationDbContext context)
        {
            _configuration = context;
        }


        public async Task<ResponseGeneral> GetAllFormAsync() {
			try
			{
                var data = new List<dynamic>();

                using (var connection = (SqlConnection)_configuration.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    string storedProcedureName = "SP_SearchTable";
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = "Form" });
                        command.Parameters.Add(new SqlParameter("@Parameters", SqlDbType.VarChar) { Value = "" });

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new ExpandoObject() as IDictionary<string, Object>;

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                     row.Add(reader.GetName(i), reader.GetValue(i));
                                }

                                data.Add(row);
                            }
                        }
                    }
                }

                List<dynamic> messages = new List<dynamic> { new Message { Text = "Búsqueda exitosa", Error = false } };
                Response responseHelper = new Response();
                ResponseGeneral response = responseHelper.ResponseSuccess(
                    status: 200,
                    messages: messages,
                    data: new List<dynamic>(data),
                    error: false
                );

                return response;


            }
            catch (Exception ex)
            {
                Response responseHelper = new Response();
                ResponseGeneral errorResponse = responseHelper.ResponseSuccess(
                    status: 500,
                    messages: new List<dynamic> { new Message { Text = $"Ocurrió un error: {ex.Message}", Error = true } },
                    data: new List<dynamic>(),
                    error: true
                );

                return errorResponse;
            }

        }


        public async Task<ResponseGeneral> GetFormAsync(int id, int audience)
        {
            try
            {
                var data = new List<dynamic>();

                using (var connection = (SqlConnection)_configuration.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    string storedProcedureName = "SP_SearchTable";
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = "Inputs" });
                        if(audience == 1)
                        {
                            command.Parameters.Add(new SqlParameter("@Parameters", SqlDbType.VarChar) { Value = $"IDForm={id} AND IsActive=1 AND IsDeleted=0" });
                        }
                        if(audience == 0)
                        {
                            command.Parameters.Add(new SqlParameter("@Parameters", SqlDbType.VarChar) { Value = $"IDForm={id} AND IsDeleted=0" });
                        }

                        if (audience != 1 && audience != 0) {
                            throw new Exception("Parámetros inválidos");
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new ExpandoObject() as IDictionary<string, Object>;

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row.Add(reader.GetName(i), reader.GetValue(i));
                                }

                                data.Add(row);
                            }
                        }
                    }
                }

                List<dynamic> messages = new List<dynamic> { new Message { Text = "Búsqueda exitosa", Error = false } };
                Response responseHelper = new Response();
                ResponseGeneral response = responseHelper.ResponseSuccess(
                    status: 200,
                    messages: messages,
                    data: new List<dynamic>(data),
                    error: false
                );

                return response;

            }
            catch (Exception ex)
            {
                Response responseHelper = new Response();
                ResponseGeneral errorResponse = responseHelper.ResponseSuccess(
                    status: 500,
                    messages: new List<dynamic> { new Message { Text = $"Ocurrió un error: {ex.Message}", Error = true } },
                    data: new List<dynamic>(),
                    error: true
                );

                return errorResponse;
            }

        }

        public async Task<ResponseGeneral> PostCreateForm(CreateTableRequest form)
        {
            try
            {
                var data = new List<dynamic>();
                
                using (var connection = (SqlConnection)_configuration.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    object? newId;

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string NameForm = form.name.Replace(" ", "");

                            // Insert name form
                            string storedProcedureName = "SP_AddDataInTable";
                            using (var command = new SqlCommand(storedProcedureName, connection, transaction))
                            {
                                
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.VarChar) { Value = "Form" });
                                command.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = "FormName" });
                                command.Parameters.Add(new SqlParameter("@Values", SqlDbType.VarChar) { Value = NameForm });

                                using (var reader = await command.ExecuteReaderAsync())
                                {
                                    if (await reader.ReadAsync())
                                    {
                                        newId = reader["ID"];
                                    } else {
                                        throw new Exception("No se guardo correctamente el registro");
                                    }
                                }
                            }

                            // Create table
                            string columns = "";
                            foreach (Inputs input in form.inputs)
                            {
                                var type = "";
                                string NameInput = input.name.Replace(" ", "");

                                switch (input.type)
                                {
                                    case "text":
                                        type = "NVARCHAR(MAX)";
                                        break;
                                    case "number":
                                        type = "INT";
                                        break;
                                    case "date":
                                        type = "DateTime";
                                        break;
                                }
                                columns += $"{NameInput} {type}, ";
                            }
                            columns += $" IDForm INT, FOREIGN KEY (IDForm) REFERENCES Form(ID) ";

                            using (var command2 = new SqlCommand("SP_CreateTable", connection, transaction))
                            {
                                command2.CommandType = CommandType.StoredProcedure;
                                command2.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = NameForm });
                                command2.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = $"{columns}" });

                                await command2.ExecuteNonQueryAsync();
                            }

                            // Insert inputs
                            foreach (Inputs newinput in form.inputs)
                            {
                                string NameInput = newinput.name.Replace(" ", "");
                                string type = newinput.type;
                                var value = $"'{NameInput}', '{type}', {newId}";
                                using (var command3 = new SqlCommand("SP_AddDataInputs", connection, transaction))
                                {
                                    command3.CommandType = CommandType.StoredProcedure;
                                    command3.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = "InputsName, InputsType, IDForm" });
                                    command3.Parameters.Add(new SqlParameter("@Values", SqlDbType.VarChar) { Value = value });

                                    await command3.ExecuteNonQueryAsync();
                                }
                            }

                            // Commit the transaction if all commands succeed
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction if any command fails
                            transaction.Rollback();
                            throw;
                        }
                    }


                }

                List<dynamic> messages = new List<dynamic> { new Message { Text = "Creación exitosa", Error = false } };
                Response responseHelper = new Response();
                ResponseGeneral response = responseHelper.ResponseSuccess(
                    status: 200,
                    messages: messages,
                    data: new List<dynamic>(),
                    error: false
                );

                return response;


            }
            catch (Exception ex)
            {
                Response responseHelper = new Response();
                ResponseGeneral errorResponse = responseHelper.ResponseSuccess(
                    status: 500,
                    messages: new List<dynamic> { new Message { Text = $"Ocurrió un error: {ex.Message}", Error = true } },
                    data: new List<dynamic>(),
                    error: true
                );

                return errorResponse;
            }

        }


        public async Task<ResponseGeneral> PostEditForm(EditTableRequest form)
        {
            try
            {
                var data = new List<dynamic>();

                using (var connection = (SqlConnection)_configuration.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {

                            // Insert inputs
                            foreach (InputsEdit Input in form.inputs)
                            {
                                string NameInput = Input.name.Replace(" ", "");
                               
                                string type = "";

                                switch (Input.type)
                                {
                                    case "text":
                                        type = "NVARCHAR(MAX)";
                                        break;
                                    case "number":
                                        type = "INT";
                                        break;
                                    case "date":
                                        type = "DateTime";
                                        break;
                                }


                                

                                if (Input.ID != null)
                                {
                                    bool isActive = Input.IsActive ?? false;
                                    bool isDeleted = Input.IsDeleted ?? false;

                                    int active = isActive ? 1 : 0;
                                    int deleted = isDeleted ? 1 : 0;



                                    using (var command = new SqlCommand("SP_EditInputs", connection, transaction))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.Add(new SqlParameter("@InputNameNew", SqlDbType.VarChar) { Value = NameInput });
                                        command.Parameters.Add(new SqlParameter("@InputId", SqlDbType.Int) { Value = Input.ID });
                                        command.Parameters.Add(new SqlParameter("@FormId", SqlDbType.Int) { Value = Input.IDForm });
                                        command.Parameters.Add(new SqlParameter("@FormType", SqlDbType.VarChar) { Value = type });
                                        command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Int) { Value = active });
                                        command.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Int) { Value = deleted });

                                        await command.ExecuteNonQueryAsync();
                                    }
                                } else {
                                    var value = $"'{NameInput}', '{type}', {form.Id}";
                                    using (var command2 = new SqlCommand("SP_AddDataInputs", connection, transaction))
                                    {
                                        command2.CommandType = CommandType.StoredProcedure;
                                        command2.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = "InputsName, InputsType, IDForm" });
                                        command2.Parameters.Add(new SqlParameter("@Values", SqlDbType.VarChar) { Value = value });

                                        await command2.ExecuteNonQueryAsync();
                                    }
                                }

                               
                            }

                            // Commit the transaction if all commands succeed
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction if any command fails
                            transaction.Rollback();
                            throw;
                        }
                    }


                }

                List<dynamic> messages = new List<dynamic> { new Message { Text = "Actualización exitosa", Error = false } };
                Response responseHelper = new Response();
                ResponseGeneral response = responseHelper.ResponseSuccess(
                    status: 200,
                    messages: messages,
                    data: new List<dynamic>(),
                    error: false
                );

                return response;


            }
            catch (Exception ex)
            {
                Response responseHelper = new Response();
                ResponseGeneral errorResponse = responseHelper.ResponseSuccess(
                    status: 500,
                    messages: new List<dynamic> { new Message { Text = $"Ocurrió un error: {ex.Message}", Error = true } },
                    data: new List<dynamic>(),
                    error: true
                );

                return errorResponse;
            }

        }
    }
}

