using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace LabWork14.Models
{
    public class Phone
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "Фамилия"), MaxLength(30)]
        public string LastName { get; set; }

        [Required, Display(Name = "Имя"), MaxLength(30)]
        public string FirstName { get; set; }

        [Required, Display(Name = "Отчество"), MaxLength(30)]
        public string MiddleName { get; set; }

        [Required, Display(Name = "Дата рождения"), DataType(DataType.Date)]
        [DateOfBirthValidator(MaxAge = 120)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required, Display(Name = "Телефонный номер"), MaxLength(10)]
        [NumberValidator]
        public string Number { get; set; }
    }

    public class PhoneDbContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
    }

    public class DateOfBirthValidator : ValidationAttribute
    {
        public int MaxAge { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var date = DateTime.Parse(value.ToString());
                var currentDate = DateTime.Now;
                if (date.Date < currentDate.Date && date.Date > currentDate.AddYears(-MaxAge).Date)
                    return ValidationResult.Success;
                throw new Exception();
            }
            catch
            {
                return new ValidationResult("Недопустимая дата рождения");
            }
        }
    }

    public class NumberValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var number = value.ToString();
                using (var db = new PhoneDbContext())
                    if (db.Phones.FirstOrDefault(i => i.Number == number) == null)
                        return ValidationResult.Success;
                throw new Exception();
            }
            catch
            {
                return new ValidationResult("Этот телефонный номер занят");
            }
        }
    }
}