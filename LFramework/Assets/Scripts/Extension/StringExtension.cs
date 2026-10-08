using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtension
{
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    /// <summary>
    /// 获取字符串的长度(一个中文两个字符)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetLength(this string str)
    {
        return Encoding.Default.GetBytes(str).Length;
    }

    /// <summary>
    /// 是否int型
    /// </summary>
    public static bool IsInt(this string str)
    {
        int n;
        return int.TryParse(str, out n);
    }

    /// <summary>
    /// 转Int 转换失败返回0
    /// </summary>
    public static int ToInt(this string str)
    {
        int n;
        if (!int.TryParse(str, out n))
            return 0;
        return n;
    }

    /// <summary>
    /// 字符串是否存在数字
    /// </summary>
    public static bool IsNumber(this string str)
    {
        return Regex.IsMatch(str, "^[0-9]+$");
    }

    /// <summary>
    /// 字符串是否存在中文
    /// </summary>
    public static bool IsChinese(this string str)
    {
        return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
    }

    /// <summary>
    /// 字符串是否以数字开头
    /// </summary>
    public static bool IsStartWithNumber(this string str)
    {
        return char.IsDigit(str.First());
    }

    /// <summary>
    /// 字符串里是否存在空格
    /// </summary>
    public static bool IsContainSpace(this string str)
    {
        return str.Contains(" ");
    }

    /// <summary>
    /// 字符串里是否包含大写字母
    /// </summary>
    public static bool IsContainCapital(this string str)
    {
        return Regex.IsMatch(str, "[A-Z]");
    }

    /// <summary>
    /// 字符串里是否包含字母
    /// </summary>
    public static bool IsContainLetter(this string str)
    {
        return Regex.Matches(str, "[a-zA-Z]").Count > 0;
    }

    /// <summary>
    /// 字符串是否包含非法字符  limitStr = @" \  / "" : * ?  < >  | ";
    /// </summary>
    public static bool IsHaveIllegalPunctuation(this string str)
    {
        return str.Trim().IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
    }

    /// <summary>
    /// 删除指定字符
    /// </summary>
    public static string RemoveTargetStr(this string str, params string[] targets)
    {
        return targets.Aggregate(str, (current, t) => current.Replace(t, string.Empty));
    }

    /// <summary>
    /// 查找两个字符串中间的字符串
    /// </summary>
    /// <param name="first">前面</param>
    /// <param name="last">后面</param>
    public static string FindBetweenStr(this string str, string first, string last)
    {
        var firstIndex = str.IndexOf(first) + last.Length;
        var lastIndex = str.IndexOf(last);
        if (firstIndex < 0 || lastIndex < 0)
            return str;
        return str.Substring(firstIndex, lastIndex - firstIndex);
    }

    /// <summary>
    /// 查找在某字符串后面的字符串
    /// </summary>
    public static string FindAfterStr(this string str, string first)
    {
        var firstIndex = str.IndexOf(first) + first.Length;
        return firstIndex < 0 ? str : str.Substring(firstIndex);
    }

    /// <summary>
    /// 拆分字符串并去除空格
    /// </summary>
    /// <param name="str"></param>
    /// <param name="cs"></param>
    /// <returns></returns>
    public static string[] SplitAndTrim(this string str, params char[] cs)
    {
        var res = str.Split(cs);
        for (int i = 0; i < cs.Length; i++)
        {
            res[i] = res[i].Trim();
        }
        return res;
    }

    /// <summary>
    /// 字符串转化为枚举
    /// </summary>
    public static T AsEnum<T>(this string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }

    #region Path操作 
    /// <summary>  
    /// 更改路径字符串的扩展名  
    /// </summary>  
    /// <param name="str"></param>  
    /// <param name="extend"></param>  
    /// <returns></returns>  
    public static string ChangeExtension(this string str, string extend)
    {
        return Path.ChangeExtension(str, extend);
    }

    /// <summary>  
    /// 返回指定路径字符串的目录信息  
    /// 文件路径：E:\Randy0528\中文目录\JustTest.rar  
    /// </summary>  
    /// <param name="str"></param>  
    /// <returns>E:\Randy0528\中文目录</returns>  
    public static string GetDirectoryName(this string str)
    {
        return Path.GetDirectoryName(str);
    }

    /// <summary>  
    /// 返回指定的路径字符串的扩展名  
    /// 文件路径：E:\Randy0528\中文目录\JustTest.rar  
    /// </summary>  
    /// <param name="str"></param>  
    /// <returns>.rar</returns>  
    public static string GetExtension(this string str)
    {
        return Path.GetExtension(str);
    }

    /// <summary>  
    /// 返回指定路径字符串的文件名和扩展名  
    /// 文件路径：E:\Randy0528\中文目录\JustTest.rar  
    /// </summary>  
    /// <param name="str"></param>  
    /// <returns>JustTest.rar</returns>  
    public static string GetFileName(this string str)
    {
        return Path.GetFileName(str);
    }

    /// <summary>  
    /// 返回不具有扩展名的指定路径字符串的文件名  
    /// 文件路径：E:\Randy0528\中文目录\JustTest.rar  
    /// </summary>  
    /// <param name="str"></param>  
    /// <returns>JustTest</returns>  
    public static string GetFileNameWithoutExtension(this string str)
    {
        return Path.GetFileNameWithoutExtension(str);
    }

    /// <summary>  
    /// 获取指定路径的根目录信息  
    /// 文件路径：E:\Randy0528\中文目录\JustTest.rar  
    /// </summary>  
    /// <param name="str"></param>  
    /// <returns>E:\</returns>  
    public static string GetPathRoot(this string str)
    {
        return Path.GetPathRoot(str);
    }

    /// <summary>  
    /// 确定路径是否包括文件扩展名  
    /// </summary>  
    /// <param name="str"></param>  
    /// <returns></returns>  
    public static bool HasExtension(this string str)
    {
        return Path.HasExtension(str);
    }

    /// <summary>  
    /// 路径字符串是包含绝对路径信息还是包含相对路径信息  
    /// </summary>  
    /// <param name="str"></param>  
    /// <returns></returns>  
    public static bool IsPathRooted(this string str)
    {
        return Path.IsPathRooted(str);
    }
    #endregion
}
