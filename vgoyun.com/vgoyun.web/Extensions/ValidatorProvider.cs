using FluentValidation;
using FluentValidation.Validators;
using LF.Toolkit.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace vgoyun.web.Extensions
{
    public static class ValidatorProvider
    {
        /// <summary>
        /// 判断是否逗号分隔的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCommaSplitNumberStr(string value)
        {
            return Regex.IsMatch(value, @"^\d+(,|,\d+)*$");
        }

        /// <summary>
        /// 判断是否合法的QQ号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="allowEmpty"></param>
        /// <returns></returns>
        public static bool IsQQ(string str, bool allowEmpty = false)
        {
            if (allowEmpty)
            {
                return string.IsNullOrEmpty(str) || Regex.IsMatch(str, @"^[1-9]\d+$");
            }
            else
            {
                return Regex.IsMatch(str, @"^[1-9]\d+$");
            }
        }

        /// <summary>
        /// 判断是否合法的手机号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="allowEmpty"></param>
        /// <returns></returns>
        public static bool IsPhone(string str, bool allowEmpty = false)
        {
            if (allowEmpty)
            {
                return string.IsNullOrEmpty(str) || Regex.IsMatch(str, @"^1[3456789]\d{9}$");
            }
            else
            {
                return Regex.IsMatch(str, @"^1[3456789]\d{9}$");
            }
        }

        /// <summary>
        /// 判断是否合法的邮箱
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="allowEmpty"></param>
        /// <returns></returns>
        public static bool IsEmail(string str, bool allowEmpty = false)
        {
            if (allowEmpty)
            {
                return string.IsNullOrEmpty(str) || Regex.IsMatch(str, @"^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$");
            }
            else
            {
                return Regex.IsMatch(str, @"^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$");
            }
        }

        /// <summary>
        /// 使用指定验证类验证对应的实体
        /// 若验证不同过，则抛出第一个错误异常
        /// </summary>
        /// <typeparam name="V">验证类</typeparam>
        /// <typeparam name="E">实体类</typeparam>
        /// <param name="entity"></param>
        public static void ThrowIfInValidate<V, E>(E entity)
            where V : AbstractValidator<E>
        {
            var validator = InjectionContainer.Resolve<V>();
            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                //抛出第一个验证错误信息 //error.PropertyName
                var error = result.Errors[0];
                throw new ArgumentException(error.ErrorMessage);
            }
        }

        /// <summary>
        /// 验证指定条件是否成立
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        public static void ThrowIf(Func<bool> condition, string message)
        {
            if (condition.Invoke()) throw new ArgumentException(message);
        }

        /// <summary>
        /// 判断指定对象是否空
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void ThrowIfNull(object obj, string message)
        {
            if (obj == null) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证2个值是否相等
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <param name="message"></param>
        public static void ThrowIfEqual<T>(T value, T compareValue, string message)
        {
            if (value.Equals(compareValue)) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证2个值是否不相等
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <param name="message"></param>
        public static void ThrowIfNotEqual<T>(T value, T compareValue, string message)
        {
            if (!value.Equals(compareValue)) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证字符串是否空或空字符串
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        public static void ThrowIfNullOrEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证值是否小于指定的值
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <param name="message"></param>
        public static void ThrowIfLessThan(int value, int compareValue, string message)
        {
            if (value < compareValue) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证值是否小于等于指定的值
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <param name="message"></param>
        public static void ThrowIfLessThanOrEqual(int value, int compareValue, string message)
        {
            if (value <= compareValue) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证值是否大于指定的值
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <param name="message"></param>
        public static void ThrowIfMoreThan(int value, int compareValue, string message)
        {
            if (value > compareValue) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证值是否大于等于指定的值
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compareValue"></param>
        /// <param name="message"></param>
        public static void ThrowIfMoreThanOrEqual(int value, int compareValue, string message)
        {
            if (value >= compareValue) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证2个值是否不在指定范围内
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="message"></param>
        public static void ThrowIfNotBetween(int value, int from, int to, string message)
        {
            if (value < from || value > to) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证指定值是否不在长度内
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="message"></param>
        public static void ThrowIfNotInLength(string value, int min, int max, string message)
        {
            if (string.IsNullOrEmpty(value) || value.Length < min || value.Length > max) throw new ArgumentException(message);
        }

        /// <summary>
        /// 验证指定值是否不在集合内
        /// 若验证通过则抛出异常信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <param name="message"></param>
        public static void ThrowIfNotIn<T>(T value, IEnumerable<T> list, string message)
        {
            if (!list.Contains(value)) throw new ArgumentException(message);
        }
    }
}