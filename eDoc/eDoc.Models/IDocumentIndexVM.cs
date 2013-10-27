using System;
namespace eDoc.Models
{
    public interface IDocumentValidationData
    {
        bool EmailValidated { get; set; }
        bool PhoneValidated { get; set; }
        bool TokenValidated { get; set; }
    }
}
