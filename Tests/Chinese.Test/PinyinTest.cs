using Chinese.Test.Util;
using Xunit;

namespace Chinese.Test;

public class PinyinTest
{
    [Fact]
    public void SimpleTest1()
    {
        var lexicon = MySqlLexicon.UseDefault();
        var str = "免费，跨平台，开源！";
        Assert.Equal("mian3 fei4，kua4 ping2 tai2，kai1 yuan2！", lexicon.GetPinyin(str));
        Assert.Equal("mian fei，kua ping tai，kai yuan！", lexicon.GetPinyin(str, PinyinFormat.WithoutTone));
        Assert.Equal("miǎn fèi，kuà píng tái，kāi yuán！", lexicon.GetPinyin(str, PinyinFormat.Phonetic));
        Assert.Equal("mf，kpt，ky！", lexicon.GetPinyin(str, PinyinFormat.InitialConsonant));
    }

    [Fact]
    public void SimpleTest2()
    {
        var lexicon = MySqlLexicon.UseDefault();
        var str = "Chinese是基于.NET平台的Chinese库。";
        Assert.Equal("Chinese shi4 ji1 yu2 .NET ping2 tai2 de5 Chinese ku4。", lexicon.GetPinyin(str));
        Assert.Equal("Chinese shi ji yu .NET ping tai de Chinese ku。", lexicon.GetPinyin(str, PinyinFormat.WithoutTone));
        Assert.Equal("Chinese shì jī yú .NET píng tái de Chinese kù。", lexicon.GetPinyin(str, PinyinFormat.Phonetic));
        Assert.Equal("Chinese sjy .NET ptd Chinese k。", lexicon.GetPinyin(str, PinyinFormat.InitialConsonant));
    }

    [Fact]
    public void NormalTest()
    {
        var lexicon = MySqlLexicon.UseDefault();
        var str = "温度设定为1度。";

        // 错误, 需要词性分析
        //TODO: 为 = wéi
        Assert.Equal("wen1 du4 she4 ding4 wei4 1 du4。", lexicon.GetPinyin(str));
    }

    [Fact]
    public void ChineseLexiconTest1()
    {
        var lexicon = MySqlLexicon.UseDefault();
        var str = "他是来自重庆的重量级选手。";
        Assert.Equal("ta1 shi4 lai2 zi4 chong2 qing4 de5 zhong4 liang4 ji2 xuan3 shou3。", lexicon.GetPinyin(str));
    }

    [Fact]
    public void NotSameWordTest()
    {
        var lexicon = MySqlLexicon.UseDefault();
        Assert.Equal("伺服器中止了服務。", lexicon.ToTraditional("服务器中止了服务。"));
        Assert.Equal("fu2 wu4 qi4 zhong1 zhi3 le5 fu2 wu4。", lexicon.GetPinyin("服务器中止了服务。", ChineseType.Simplified));
        Assert.Equal("si4 fu2 qi4 zhong1 zhi3 le5 fu2 wu4。", lexicon.GetPinyin("伺服器中止了服務。", ChineseType.Traditional));
    }

    [Fact]
    public void ParagraghTest()
    {
        var lexicon = MySqlLexicon.UseDefault();
        var text = @"这地方的火烧云变化极多，一会儿红彤彤的，一会儿金灿灿的，一会儿半紫半黄，一会儿半灰半百合色。葡萄灰、梨黄、茄子紫，这些颜色天空都有。还有些说也说不出来、见也没见过的颜色。

一会儿，天空出现一匹马，马头向南，马尾向西。马是跪着的，像等人骑上它的背，它才站起来似的。过了两三秒钟，那匹马大起来了，腿伸开了，脖子也长了，尾巴却不见了。看的人正在寻找马尾巴，那匹马变模糊了。

忽然又来了一条大狗。那条狗十分凶猛，在向前跑，后边似乎还跟着好几条小狗。跑着跑着，小狗不知哪里去了，大狗也不见了。

接着又来了一头大狮子，跟庙门前的石头狮子一模一样，也那么大，也那样蹲着，很威武很镇静地蹲着。可是一转眼就变了，再也找不着了。";

        var pinyin = lexicon.GetPinyin(text, ChineseType.Simplified, PinyinFormat.Phonetic);

        // 错误, 需要词性分析
        //TODO: 的背 = de / bēi
        //TODO: 还跟着 = huán / gēn zhe
        Assert.Equal(
"""
zhè dì fāng de huǒ shāo yún biàn huà jí duō，yī huì er hóng tóng tóng de，yī huì er jīn càn càn de，yī huì er bàn zǐ bàn huáng，yī huì er bàn huī bàn bǎi hé sè。pú táo huī、lí huáng、qié zi zǐ，zhè xiē yán sè tiān kōng dōu yǒu。huán yǒu xiē shuō yě shuō bù chū lái、jiàn yě méi jiàn guò de yán sè。

yī huì er，tiān kōng chū xiàn yī pǐ mǎ，mǎ tóu xiàng nán，mǎ wěi xiàng xī。mǎ shì guì zhe de，xiàng děng rén qí shàng tā de bēi，tā cái zhàn qǐ lái shì de。guò le liǎng sān miǎo zhōng，nà pǐ mǎ dà qǐ lái le，tuǐ shēn kāi le，bó zi yě zhǎng le，wěi bā què bù jiàn le。kàn de rén zhèng zài xún zhǎo mǎ wěi bā，nà pǐ mǎ biàn mó hú le。

hū rán yòu lái le yī tiáo dà gǒu。nà tiáo gǒu shí fēn xiōng měng，zài xiàng qián pǎo，hòu biān sì hū huán gēn zhe hǎo jǐ tiáo xiǎo gǒu。pǎo zhe pǎo zhe，xiǎo gǒu bù zhī nǎ lǐ qù le，dà gǒu yě bù jiàn le。

jiē zhe yòu lái le yī tóu dà shī zi，gēn miào mén qián de shí tou shī zi yī mú yī yàng，yě nà me dà，yě nà yàng dūn zhe，hěn wēi wǔ hěn zhèn jìng dì dūn zhe。kě shì yī zhuǎn yǎn jiù biàn le，zài yě zhǎo bù zháo le。
"""
        , pinyin);
    }

}
