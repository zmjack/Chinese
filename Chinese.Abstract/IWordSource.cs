﻿namespace Chinese;

public interface IWordSource
{
    string DbFileName { get; }
    string ResourceName { get; }

    void CreateDbFile();
    ChineseChar[] GetChars(ChineseType type, IEnumerable<char> chars);
    ChineseWord[] GetWords(ChineseType type, IEnumerable<char> chars);
}

public static class WordSourceExtensions
{
    public static bool IsDbFileExsist(this IWordSource @this) => File.Exists(@this.DbFileName);
}
