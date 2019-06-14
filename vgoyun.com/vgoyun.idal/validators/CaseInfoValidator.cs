using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal.validators
{
    public class CaseInfoValidator : AbstractValidator<CaseInfo>
    {
        public CaseInfoValidator()
        {
            //typeid
            RuleFor(i => i.typeid).GreaterThanOrEqualTo(0).WithMessage("类型不能为空");
            //imgurl
            RuleFor(i => i.imgurl).NotEmpty().WithMessage("图片地址不能为空");
            RuleFor(i => i.imgurl).Length(1, 500).WithMessage("图片地址长度为1~500个字符串");
            //title
            RuleFor(i => i.title).NotEmpty().WithMessage("标题不能为空");
            RuleFor(i => i.title).Length(1, 100).WithMessage("标题长度为1~100个字符串");
            //link
            RuleFor(i => i.link).NotEmpty().WithMessage("链接不能为空");
            RuleFor(i => i.link).Length(1, 500).WithMessage("链接长度为1~500个字符串");
            //seecount
            RuleFor(i => i.seecount).GreaterThanOrEqualTo(0).WithMessage("查看数不能小于0");
            //prizecount
            RuleFor(i => i.prizecount).GreaterThanOrEqualTo(0).WithMessage("点赞数不能小于0");
            //sort
            RuleFor(i => i.sort).GreaterThanOrEqualTo(0).WithMessage("排序值不能小于0");
        }
    }
}
