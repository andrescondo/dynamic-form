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
                    // Insert name form
                    string storedProcedureName = "AddDataInTable";
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.VarChar) { Value = "Form" });
                        command.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = "FormName" });
                        command.Parameters.Add(new SqlParameter("@Values", SqlDbType.VarChar) { Value = form.name });

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var newId = reader["ID"];
                                connection.Close();

                                //create table
                                using (var connection2 = (SqlConnection)_configuration.Database.GetDbConnection())
                                {
                                    await connection2.OpenAsync();
                                    using (var command2 = new SqlCommand("CreateTable", connection2))
                                    {
                                        command2.CommandType = CommandType.StoredProcedure;
                                        string columns = "";
                                        command2.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar) { Value = form.name });
                                        foreach (Inputs input in form.inputs) {
                                            var type = "";
                                            switch(input.type)
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
                                            columns += $"{input.name} {type}, ";
                                        }
                                        columns += $" IDForm INT, FOREIGN KEY (IDForm) REFERENCES Form(ID) ";
                                        command2.Parameters.Add(new SqlParameter("@Columns", SqlDbType.VarChar) { Value = $"{columns}" });

                                        using (var reader2 = await command2.ExecuteReaderAsync())
                                        {
                                            if (await reader2.ReadAsync())
                                            {
                                                connection2.Close();
                                            }
                                        }
                                    }

                                }
                                


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
    }
}

