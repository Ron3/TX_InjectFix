using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.GZip;

namespace ETModel
{
    public static class BPZipHelper
    {
        
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] inputBytes)
        {    
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipOutputStream gzipOut = new GZipOutputStream(outStream))
                {
                    //很重要，必须关闭，否则无法正确解压
                    gzipOut.Write(inputBytes, 0, inputBytes.Length);
                    Log.Debug("GetLevel ==> " + gzipOut.GetLevel());
                    gzipOut.Flush();
                    gzipOut.Close();
                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] inputBytes)
        {
            using (MemoryStream inputStream = new MemoryStream(inputBytes))
            {
                inputStream.Write(inputBytes, 0, inputBytes.Length);
                inputStream.Seek(0, SeekOrigin.Begin);

                using(MemoryStream decompMemStream = new MemoryStream())
                {
                    using (GZipInputStream gzipInput = new GZipInputStream(inputStream))
                    {
                        int size = 2048;
                        byte[] buffer = new byte[size];
                        while (size > 0)
                        {
                            size = gzipInput.Read(buffer, 0, size);
                            if (size > 0)
                                decompMemStream.Write(buffer, 0, size);
                        }

                        return decompMemStream.ToArray();
                    }
                }
            }
        }

        #region ==============zlib=============
        /// <summary>
        /// 采用zlib的压缩
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] CompressZlib(byte[] inputBytes)
        {
            MemoryStream inputStream = new MemoryStream(inputBytes);
            inputStream.Position = 0;
            
            using(MemoryStream outputStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress, true))
                {
                    byte[] buf = new byte[1024];
                    int len;
                    while ((len = inputStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        Log.Debug("len ==> " + len);
                        deflateStream.Write(buf, 0, len);
                    }
                }

                return outputStream.ToArray();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] DecompressZlib(byte[] inputBytes)
        {
            MemoryStream inputStream = new MemoryStream(inputBytes) {Position = 0};
            MemoryStream outputStream = new MemoryStream();
            using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress, true))
            {
                byte[] buf = new byte[1024];
                int len;
                while ((len = deflateStream.Read(buf, 0, buf.Length)) > 0)
                    outputStream.Write(buf, 0, len);
            }

            return outputStream.ToArray();
        }
        #endregion




        #region ==============gzip==============
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] CompressGZip(byte[] inputBytes)
        {    
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    //很重要，必须关闭，否则无法正确解压
                    
                    zipStream.Write(inputBytes, 0, inputBytes.Length);
                    zipStream.Close();
                    return outStream.ToArray();
                }
            }
        }


        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <returns></returns>
        public static byte[] DecompressGZip(byte[] inputBytes)
        {
            using (MemoryStream inputStream = new MemoryStream(inputBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(outStream);
                        zipStream.Close();
                        return outStream.ToArray();
                    }
                }
            }
        }
        #endregion
    }
}
