using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.AppUsers
{
    public class ConfirmOtpDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
