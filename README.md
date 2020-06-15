# Chinese

中文拼音及数字读法转换

```c#
Assert.Equal("qu4 ba1，pi2 ka3 qiu1！", Pinyin.Get("去吧，皮卡丘！", PinyinFormat.Default));
Assert.Equal("qu ba，pi ka qiu！", Pinyin.Get("去吧，皮卡丘！", PinyinFormat.WithoutTone));
Assert.Equal("qù bā，pí kǎ qiū！", Pinyin.Get("去吧，皮卡丘！", PinyinFormat.PhoneticSymbol));
```

