using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Helpers
{
    public static class CamelToSnakeHelper
    {
        public static string CamelToSnake(this string camelCase)
        {
            StringBuilder snakeCase = new StringBuilder();
            for (int i = 0; i < camelCase.Length; i++)
            {
                char currentChar = camelCase[i];
                if (char.IsUpper(currentChar))
                {
                    if (i > 0)
                    {
                        snakeCase.Append('_');
                    }
                    snakeCase.Append(char.ToLower(currentChar));
                }
                else
                {
                    snakeCase.Append(currentChar);
                }
            }
            return snakeCase.ToString();
        }
    }
}
