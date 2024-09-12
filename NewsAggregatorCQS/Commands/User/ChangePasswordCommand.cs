using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.User
{
    public class ChangePasswordCommand : IRequest
    {
        public string Email {  get; set; }
        public string PasswordHash { get; set; }
    }
}
