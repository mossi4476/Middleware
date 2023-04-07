using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Middleware.Models
{
    public class ValidateUnique : ValidationAttribute
    {
        private readonly string DefaultConnection = "Server=DESKTOP-BKEONEI\\MSSQLSERVER05;Database=Student1;TrustServerCertificate=True;Trusted_Connection=True;";
        private readonly string _tableName;
        private readonly string _columnName;
        public ValidateUnique(string tableName, string columnName)
        {
            _tableName = tableName;
            _columnName = columnName;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            var currentValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
            using (var connection = new SqlConnection(DefaultConnection))
            {

                connection.Open();
                var command = new SqlCommand($"SELECT COUNT(*) FROM {_tableName} WHERE {_columnName} = @value", connection);
                command.Parameters.AddWithValue("@value", currentValue);
                var count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    return new ValidationResult($"{validationContext.DisplayName} must be unique.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
