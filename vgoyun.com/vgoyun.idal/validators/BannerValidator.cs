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
    public class BannerValidator : AbstractValidator<Banner>
    {
        public BannerValidator()
        {
            //title - varchar(100) [不可空]  - 标题
            RuleFor(i => i.title).NotEmpty().WithMessage("标题不能为空");
            RuleFor(i => i.title).MaximumLength(100).WithMessage("标题长度不能超过100个字符串");
            //image_url - varchar(300) [不可空]  - 图片地址
            RuleFor(i => i.image_url).NotEmpty().WithMessage("图片地址不能为空");
            RuleFor(i => i.image_url).MaximumLength(300).WithMessage("图片地址长度不能超过300个字符串");
            //link_url - varchar(300) [可空]  - 调转地址
            RuleFor(i => i.link_url).MaximumLength(300).WithMessage("跳转地址长度不能超过300个字符串");
            //sort - int(4) [不可空]  - 排序
            RuleFor(i => i.sort).GreaterThanOrEqualTo(0).WithMessage("排序值不能小于0");
            //type - int(4) [不可空]  - 图片类型 1=首页
            RuleFor(i => i.type).InclusiveBetween(1, 7).WithMessage("图片类型值不在范围内");
            //created - datetime(16) [不可空]  - 
        }
    }
}
