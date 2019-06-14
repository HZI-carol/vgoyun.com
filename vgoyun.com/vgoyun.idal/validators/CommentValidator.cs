using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal.validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            //contents
            RuleFor(i => i.contents).NotEmpty().WithMessage("内容不能为空");
            //contents
            RuleFor(i => i.contents).MaximumLength(1000).WithMessage("内容最大长度不能超过1000个字符串");
            //ipaddress
            RuleFor(i => i.ipaddress).NotEmpty().WithMessage("IP地址不能为空");
            //ipaddress
            RuleFor(i => i.ipaddress).MaximumLength(50).WithMessage("IP地址最大长度不能超过50个字符串");
        }
    }
}
