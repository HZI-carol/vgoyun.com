using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal.validators
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            //typeid
            RuleFor(i => i.typeid).GreaterThanOrEqualTo(0).WithMessage("类型不能为空");
            //imgurl
            RuleFor(i => i.imgurl).NotEmpty().WithMessage("图片地址不能为空");
            RuleFor(i => i.imgurl).Length(1, 500).WithMessage("图片地址长度为1~500个字符串");
            //author
            RuleFor(i => i.author).NotEmpty().WithMessage("作者不能为空");
            RuleFor(i => i.author).Length(1, 400).WithMessage("作者长度为1~400个字符串");
            //title
            RuleFor(i => i.title).NotEmpty().WithMessage("标题不能为空");
            RuleFor(i => i.title).Length(1, 400).WithMessage("标题长度为1~400个字符串");
            //samllcontents
            RuleFor(i => i.samllcontents).NotEmpty().WithMessage("简介不能为空");
            RuleFor(i => i.samllcontents).Length(1, 1000).WithMessage("简介长度为1~1000个字符串");
            //contents
            RuleFor(i => i.contents).NotEmpty().WithMessage("内容不能为空");
            //seecount
            RuleFor(i => i.seecount).GreaterThanOrEqualTo(0).WithMessage("查看数不能小于0");
            //sort
            RuleFor(i => i.sort).GreaterThanOrEqualTo(0).WithMessage("排序值不能小于0");
        }
    }
}
