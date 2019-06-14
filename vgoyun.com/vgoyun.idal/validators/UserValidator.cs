using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal.validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            //用户名
            RuleFor(su => su.username).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(su => su.username).Length(1, 20).WithMessage("用户名长度为1~20个字符串");
            //昵称
            RuleFor(su => su.nickname).NotEmpty().WithMessage("昵称不能为空");
            RuleFor(su => su.nickname).Length(1, 50).WithMessage("昵称长度为1~50个字符串");
            //密码
            RuleFor(su => su.password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(su => su.password).Length(6, 18).WithMessage("密码长度为6~18个字符串");
            //状态
            RuleFor(su => su.status).InclusiveBetween(0, 1).WithMessage("状态值不在范围内");
        }
    }
}
