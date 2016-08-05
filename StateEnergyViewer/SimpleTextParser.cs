using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEnergyViewer
{
    class SimpleTextParser
    {
        public List<string> Tokens { get; set; }

        public SimpleTextParser(string text)
        {
            Tokens = new List<string>();
            Tokens.Add("");

            char[] separatingCharacters = { '"', '{', '}', ':', '[', ']', ',' };
            for (int i = 0; i < text.Length; i++)
            {
                if (separatingCharacters.Contains(text[i]))
                {
                    //If multiple separating characters are in succession, this will not create blank entries
                    if (Tokens.Last() != "")
                    {
                        Tokens.Add("");
                    }
                }
                else
                {
                    Tokens[Tokens.Count - 1] = Tokens.Last() + text[i];
                }
            }
        }
    }
}
