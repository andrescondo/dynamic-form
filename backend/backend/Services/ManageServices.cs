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
        Task<ResponseGeneral> GetFormAsync(int id);
        Task<ResponseGeneral> PostCreateForm(CreateTableRequest form);

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
                    string storedProcedureName = "SearchTable";
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


        public async Task<ResponseGeneral> GetFormAsync(int id)
        {
            try
            {
                var data = new List<dynamic>();

                using (var connection = (SqlConnection)_configuration.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    string storedProcedureName = "SearchTable";
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = "Inputs" });
                        command.Parameters.Add(new SqlParameter("@Parameters", SqlDbType.VarChar) { Value = $"IDForm={id}" });

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
                            // Insert name form
                            string storedProcedureName = "AddDataInTable";
                            using (var command = new SqlCommand(storedProcedureName, connection, transaction))
                            {
                                string NewName1 = form.name.Replace(" ", "");
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.VarChar) { Value = "Form" });
                                command.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = "FormName" });
                                command.Parameters.Add(new SqlParameter("@Values", SqlDbType.VarChar) { Value = NewName1 });

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
                                string NewName2 = input.name.Replace(" ", "");

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
                                columns += $"{NewName2} {type}, ";
                            }
                            columns += $" IDForm INT, FOREIGN KEY (IDForm) REFERENCES Form(ID) ";

                            using (var command2 = new SqlCommand("CreateTable", connection, transaction))
                            {
                                command2.CommandType = CommandType.StoredProcedure;
                                command2.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = form.name });
                                command2.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = $"{columns}" });

                                await command2.ExecuteNonQueryAsync();
                            }

                            // Insert inputs
                            foreach (var newinput in form.inputs)
                            {
                                string NewName3 = newinput.name.Replace(" ", "");
                                var value = $"'{NewName3}', '{newinput.type}', {newId}";
                                using (var command3 = new SqlCommand("AddDataInputs", connection, transaction))
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
    }
}

