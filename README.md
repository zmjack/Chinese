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

### 版本计划：0.8.1-alpha

欢迎仅使用 **数字转换** / **货币转换** 功能的项目升级试用。

- [x] 大量逻辑优化，优化内存使用。

- [x] 提高 **数字转换** / **货币转换** 性能，修复一些 **BUG**（Issues: #10）。

  ```csharp
  Lexicon.Currency.GetString(0.1m);  // 零元一角整
  ```

- [x] 修复 **中文掺杂字母或拼音** 转换时缺少 **空格** 的问题（Issues: #12）。

- [ ] 数值读法增加 **小数** 读法。

- [ ] 提供外部词库支持，现可使用 **数据库** 作为外部词库源。（结构需优化）

- [ ] 词库编译生成器（解析 **文本结构** 到 **Sqlite** 数据库文件，用户可以通过文本结构提交 PR 贡献）。

### 版本：0.5.0

- 简繁转换性能优化（**NETStandard2.0+**，**NET5.0+**，通常情况耗时减少约 **90%**）。

- 计量读法性能优化（耗时减少约 **85%**）。

- 修复计量读法尾零转换错误问题：

  ```csharp
  var wrong = ChineseCurrency.GetString(10_0000);  // 错误：一十万零元整
  var right = ChineseCurrency.GetString(10_0000);  // 正确：一十万元整
  ```

### 版本：4.5.0

- 更新部分字默认拼音。
- 发布测试词库 **Chinese.Words**（v0.0.1）。

### 版本：4.1.0

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

## 简繁转换

```c#
ChineseConverter.ToTraditional("免费，跨平台，开源！");    // "免費，跨平臺，開源！"
ChineseConverter.ToSimplified("免費，跨平臺，開源！");     // "免费，跨平台，开源！"
```

<br/>

## 数字读法

| NumberMode 选项 | 描述                                                         | 示例                      |
| --------------- | ------------------------------------------------------------ | ------------------------- |
| **Default**     | 流行读法，亿级以上使用 `千亿` `万亿` 等单位；<br />不省略第一个 `十` 前的 `一` ；<br/>小写读法。 | -                         |
| **Classical**   | 经典读法，亿级以上使用 `兆` `京` 等单位                      | -                         |
| **Concise**     | 简洁读法，省略第一个 `十` 前的 `一`                          | `10_0001` 读作 `十万零一` |
| **Upper**       | 大写读法                                                     | `1` 读作 `壹`             |



### 计量读法

```c#
var lexicon = Lexicon.Number;
lexicon.GetString(10_0001);  // "一十万零一"
lexicon.GetString(10_0101);  // "一十万零一百零一"
lexicon.GetString(10_1001);  // "一十万一千零一"
lexicon.GetString(10_1010);  // "一十万一千零一十"

lexicon.GetNumber("一十万零一");        // 10_0001
lexicon.GetNumber("一十万零一百零一");  // 10_0101
lexicon.GetNumber("一十万一千零一");    // 10_1001
lexicon.GetNumber("一十万一千零一十");  // 10_1010
```

#### 大数读法

##### 流行读法

亿级以上使用（万亿、亿亿、万亿亿、亿亿亿、万亿亿亿）：

```csharp
var lexicon = Lexicon.Number;

// "一万亿亿亿二千三百四十五亿亿亿六千七百八十九万亿亿零一百二十三亿亿四千五百六十七万亿八千九百零一亿二千三百四十五万六千七百八十九"
lexicon.GetString(1_2345_6789_0123_4567_8901_2345_6789m);

// 12345678901234567890123456789
lexicon.GetNumber("一万亿亿亿二千三百四十五亿亿亿六千七百八十九万亿亿零一百二十三亿亿四千五百六十七万亿八千九百零一亿二千三百四十五万六千七百八十九");
```

##### 经典读法

亿级以上使用（兆、京、垓、秭、穰）：

```c#
var lexicon = Lexicon.NumberWith(NumberMode.Classical);

// "一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九"
lexicon.GetString(1_2345_6789_0123_4567_8901_2345_6789m);

// 12345678901234567890123456789
lexicon.GetNumber("一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九");
```

<br/>

### 编号读法

```c#
var lexicon = Lexicon.NumberWith(NumberMode.Code);
lexicon.GetString(10_0001);  // "一〇〇〇〇一"
lexicon.GetString(10_0101);  // "一〇〇一〇一"
lexicon.GetString(10_1001);  // "一〇一〇〇一"
lexicon.GetString(10_1010);  // "一〇一〇一〇"

lexicon.GetNumber("一〇〇〇〇一");  // 10_0001
lexicon.GetNumber("一〇〇一〇一");  // 10_0101
lexicon.GetNumber("一〇一〇〇一");  // 10_1001
lexicon.GetNumber("一〇一〇一〇");  // 10_1010
```

<br/>

### 货币读法

```c#
var lexicon = Lexicon.Currency;

lexicon.GetString(1);        // "一元整"
lexicon.GetString(10_0001);  // "一十万零一元整"
lexicon.GetString(10_0101);  // "一十万零一百零一元整"
lexicon.GetString(10_1001);  // "一十万一千零一元整"
lexicon.GetString(10_1010);  // "一十万一千零一十元整"
lexicon.GetString(10_0001.2m);   // "一十万零一元二角整"
lexicon.GetString(10_0001.23m);  // "一十万零一元二角三分"
lexicon.GetString(10_0001.03m);  // "一十万零一元零三分"

lexicon.GetNumber("一元整");                // 1
lexicon.GetNumber("一十万零一元整");        // 10_0001
lexicon.GetNumber("一十万零一百零一元整");  // 10_0101
lexicon.GetNumber("一十万一千零一元整");    // 10_1001
lexicon.GetNumber("一十万一千零一十元整");  // 10_1010
lexicon.GetNumber("一十万零一元二角整");    // 10_0001.2m
lexicon.GetNumber("一十万零一元二角三分");  // 10_0001.23m
lexicon.GetNumber("一十万零一元零三分");    // 10_0001.03m
```

<br/>