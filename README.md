# Chinese

中文解析通用工具。包括拼音，简繁转换，数字读法，货币读法。

<br/>

## 安装

通过 **Nuget** 使用 **.NET CLI** 安装：

```powershell
dotnet add package Chinese
```

词库安装：

```powershell
dotnet add package Chinese.Words
```

<br/>

## 版本更新

**v0.4.5**

- 更新部分字默认拼音。
- 发布测试词库 **Chinese.Words**（v0.0.1）。

**v0.4.2**

- 支持更多 **.NET** 实现：**Net35**、**Net40**、**Net45**、**Net451**、**Net46**、**Net461**。

**v0.4.1**

- 按词频为单字提供默认拼音。

<br/>

## 拼音

```c#
// "mian3 fei4，kua4 ping2 tai2，kai1 yuan2！"
Pinyin.GetString("免费，跨平台，开源！", PinyinFormat.Default);
```

```c#
// "mian fei，kua ping tai，kai yuan！"
Pinyin.GetString("免费，跨平台，开源！", PinyinFormat.WithoutTone);
```

```c#
// "miǎn fèi，kuà píng tái，kāi yuán！"
Pinyin.GetString("免费，跨平台，开源！", PinyinFormat.Phonetic);
```

```c#
// "mf，kpt，ky！"
Pinyin.GetString("免费，跨平台，开源！", PinyinFormat.InitialConsonant);
```

<br/>

使用词库 [Chinese.Words](https://github.com/zmjack/Chinese/blob/master/Chinese.Words/README.md) 示例 ：

```csharp
var lexicon = new ChineseLexicon(Additional.CommonWords, Builtin.ChineseChars);
using (lexicon.BeginScope())
{
    var str = Pinyin.GetString("只有这只鸟跑得很快。");	
    // zhi3 you3 zhe4 zhi1 niao3 pao3 de2 hen3 kuai4。
    var words = ChineseTokenizer.SplitWords("只有这只鸟跑得很快。");
    // 只有 / 这只 / 鸟 / 跑 / 得 / 很快 / 。
}
```

<br/>

## 简繁转换

```c#
ChineseConverter.ToTraditional("免费，跨平台，开源！");    // "免費，跨平臺，開源！"
ChineseConverter.ToSimplified("免費，跨平臺，開源！");     // "免费，跨平台，开源！"
```

<br/>

## 数字读法

### 计量读法

小写读法：

```c#
var options = new ChineseNumberOptions { Simplified = false, Upper = false };
ChineseNumber.GetString(10_0001, options);    // "一十万零一"
ChineseNumber.GetString(10_0101, options);    // "一十万零一百零一"
ChineseNumber.GetString(10_1001, options);    // "一十万一千零一"
ChineseNumber.GetString(10_1010, options);    // "一十万一千零一十"
```

```c#
var options = new ChineseNumberOptions { Simplified = true, Upper = false };
ChineseNumber.GetString(10_0001, options);    // "十万零一"
ChineseNumber.GetString(10_0101, options);    // "十万零一百零一"
ChineseNumber.GetString(10_1001, options);    // "十万一千零一"
ChineseNumber.GetString(10_1010, options);    // "十万一千零一十"
```
大写读法：

```c#
var options = new ChineseNumberOptions { Simplified = false, Upper = true };
ChineseNumber.GetString(10_0001, options);    // "壹拾万零壹"
ChineseNumber.GetString(10_0101, options);    // "壹拾万零壹佰零壹"
ChineseNumber.GetString(10_1001, options);    // "壹拾万壹仟零壹"
ChineseNumber.GetString(10_1010, options);    // "壹拾万壹仟零壹拾"
```

```c#
var options = new ChineseNumberOptions { Simplified = true, Upper = true };
ChineseNumber.GetString(10_0001, options);    // "拾万零壹"
ChineseNumber.GetString(10_0101, options);    // "拾万零壹佰零壹"
ChineseNumber.GetString(10_1001, options);    // "拾万壹仟零壹"
ChineseNumber.GetString(10_1010, options);    // "拾万壹仟零壹拾"
```

大数读法（万、亿、兆、京、垓、秭、穰）：

```c#
// "一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九"
ChineseNumber.GetString(1_2345_6789_0123_4567_8901_2345_6789m);
```

```c#
// "壹穰贰仟叁佰肆拾伍秭陆仟柒佰捌拾玖垓零壹佰贰拾叁京肆仟伍佰陆拾柒兆捌仟玖佰零壹亿贰仟叁佰肆拾伍万陆仟柒佰捌拾玖"
ChineseNumber.GetString(1_2345_6789_0123_4567_8901_2345_6789m, x => x.Upper = true);
```

中文读法转数值：

```c#
ChineseNumber.GetNumber("一十万零一");          // 10_0001
ChineseNumber.GetNumber("一十万零一百零一");    // 10_0101
ChineseNumber.GetNumber("一十万一千零一");      // 10_1001
ChineseNumber.GetNumber("一十万一千零一十");    // 10_1010
```
```c#
// 1_2345_6789_0123_4567_8901_2345_6789
ChineseNumber.GetNumber("一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九");
```

自定义分级单位：

```c#
ChineseNumber.SuperiorLevels = new[] { "", "万", "亿", "万亿", "亿亿", "万亿亿", "亿亿亿", "万亿亿亿" };            
ChineseNumber.GetString(30_0020_0000_0001);    // 三十万亿零二十亿零一
```

<br/>

### 编号读法

```c#
ChineseNumber.GetCodeString(10_0001.ToString(), upper: false);    // "一〇〇〇〇一"
ChineseNumber.GetCodeString(10_0101.ToString(), upper: false);    // "一〇〇一〇一"
ChineseNumber.GetCodeString(10_1001.ToString(), upper: false);    // "一〇一〇〇一"
ChineseNumber.GetCodeString(10_1010.ToString(), upper: false);    // "一〇一〇一〇"
```

```c#
ChineseNumber.GetCodeString(10_0001.ToString(), upper: true);     // "壹零零零零壹"
ChineseNumber.GetCodeString(10_0101.ToString(), upper: true);     // "壹零零壹零壹"
ChineseNumber.GetCodeString(10_1001.ToString(), upper: true);     // "壹零壹零零壹"
ChineseNumber.GetCodeString(10_1010.ToString(), upper: true);     // "壹零壹零壹零"
```

中文读法转数值编号：

```c#
ChineseNumber.GetCodeNumber("一〇〇〇〇一");    // "100001"
ChineseNumber.GetCodeNumber("一〇〇一〇一");    // "100101"
ChineseNumber.GetCodeNumber("一〇一〇〇一");    // "101001"
ChineseNumber.GetCodeNumber("一〇一〇一〇");    // "101010"
```
<br/>

### 货币读法

货币小写读法：

```c#
var options = new ChineseNumberOptions { Simplified = false, Upper = false };
ChineseCurrency.GetString(10_0001, options);       // "一十万零一元整"
ChineseCurrency.GetString(10_0101, options);       // "一十万零一百零一元整"
ChineseCurrency.GetString(10_1001, options);       // "一十万一千零一元整"
ChineseCurrency.GetString(10_1010, options);       // "一十万一千零一十元整"
ChineseCurrency.GetString(10_0001.2m, options);    // "一十万零一元二角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "一十万零一元二角三分"
ChineseCurrency.GetString(10_0001.03m, options);   // "一十万零一元零三分"
```

```c#
var options = new ChineseNumberOptions { Simplified = true, Upper = false };
ChineseCurrency.GetString(10_0001, options);       // "十万零一元整"
ChineseCurrency.GetString(10_0101, options);       // "十万零一百零一元整"
ChineseCurrency.GetString(10_1001, options);       // "十万一千零一元整"
ChineseCurrency.GetString(10_1010, options);       // "十万一千零一十元整"
ChineseCurrency.GetString(10_0001.2m, options);    // "十万零一元二角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "十万零一元二角三分"
ChineseCurrency.GetString(10_0001.03m, options);   // "十万零一元零三分"
```

货币大写读法：

```c#
var options = new ChineseNumberOptions { Simplified = false, Upper = true };
ChineseCurrency.GetString(10_0001, options);       // "壹拾万零壹圆整"
ChineseCurrency.GetString(10_0101, options);       // "壹拾万零壹佰零壹圆整"
ChineseCurrency.GetString(10_1001, options);       // "壹拾万壹仟零壹圆整"
ChineseCurrency.GetString(10_1010, options);       // "壹拾万壹仟零壹拾圆整"
ChineseCurrency.GetString(10_0001.2m, options);    // "壹拾万零壹圆贰角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "壹拾万零壹圆贰角叁分"
ChineseCurrency.GetString(10_0001.03m, options);   // "壹拾万零壹圆零叁分"
```

```c#
var options = new ChineseNumberOptions { Simplified = true, Upper = true };
ChineseCurrency.GetString(10_0001, options);       // "拾万零壹圆整"
ChineseCurrency.GetString(10_0101, options);       // "拾万零壹佰零壹圆整"
ChineseCurrency.GetString(10_1001, options);       // "拾万壹仟零壹圆整"
ChineseCurrency.GetString(10_1010, options);       // "拾万壹仟零壹拾圆整"
ChineseCurrency.GetString(10_0001.2m, options);    // "拾万零壹圆贰角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "拾万零壹圆贰角叁分"
ChineseCurrency.GetString(10_0001.03m, options);   // "拾万零壹圆零叁分"
```

中文读法转货币数值：

```c#
ChineseCurrency.GetNumber("一十万零一元整");          // 10_0001
ChineseCurrency.GetNumber("一十万零一百零一元整");    // 10_0101
ChineseCurrency.GetNumber("一十万一千零一元整");      // 10_1001
ChineseCurrency.GetNumber("一十万一千零一十元整");    // 10_1010
ChineseCurrency.GetNumber("一十万零一元二角整");      // 10_0001.2m
ChineseCurrency.GetNumber("一十万零一元二角三分");    // 10_0001.23m
ChineseCurrency.GetNumber("一十万零一元零三分");      // 10_0001.03m
```
<br/>