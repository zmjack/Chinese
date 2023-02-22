using Chinese.Core;
using Chinese.Data;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese.Test.Util
{
    internal static class DefaultLexicon
    {
        public static Lexicon Instance = Any.Create(() =>
        {
            var context = ApplicationDbContext.UseMySql("server=127.0.0.1;port=33306;user=root;password=root;database=chinese");
            return new Lexicon(context.Words.AsQueryable<IWord>(), context.Chars.AsQueryable<IWord>());
        });
    }
}
