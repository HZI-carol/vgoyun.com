using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal.validators
{
    public class IntentionValidator : AbstractValidator<Intention>
    {
        public IntentionValidator()
        {
            //name - varchar(20) [不可空]  - 姓名
            RuleFor(i => i.name).NotEmpty().WithMessage("姓名不能为空");
            RuleFor(i => i.name).MaximumLength(20).WithMessage("姓名长度不能超过20个字符串");
            //phone - varchar(20) [不可空]  - 手机号
            RuleFor(i => i.phone).NotEmpty().WithMessage("手机号不能为空");
            RuleFor(i => i.phone).MaximumLength(20).WithMessage("手机号长度不能超过20个字符串");
            //intentions - varchar(30) [不可空]  - 意向度(多个以逗号分隔)
            RuleFor(i => i.intention).NotEmpty().WithMessage("意向度不能为空");
            RuleFor(i => i.intention).MaximumLength(30).WithMessage("意向度长度不能超过30个字符串");
            RuleFor(i => i.remark).MaximumLength(500).WithMessage("备注长度不能超过500个字符串");
            //created - datetime(16) [不可空]  - 创建时间
        }
    }
}
