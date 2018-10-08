using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy
{
        public static class ExtensionMethods
        {
            public static byte[] Combine(this byte[] bytes, byte[] b, int count)
            {
                byte[] data = new byte[bytes.Length + count];
                Buffer.BlockCopy(bytes, 0, data, 0, bytes.Length);
                Buffer.BlockCopy(b, 0, data, bytes.Length, count);
                return data;
            }
        }

    

    
}
