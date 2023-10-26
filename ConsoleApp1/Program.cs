using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Linq;
using System.Text.Json.Nodes;


class MainClass
{

    static void Main()
    {

        WebClient client = new WebClient();
        string s = client.DownloadString("https://coderbyte.com/api/challenges/json/json-cleaning");

        Console.WriteLine(s);

        var jsonObject = (JObject)JsonConvert.DeserializeObject(s);

        RemoveSpecificProperties(jsonObject);

        Console.WriteLine(jsonObject.ToString());
           
        Console.ReadLine();
    }

    static void RemoveSpecificProperties(JObject jsonObject)
    {
        var properties = jsonObject.Properties().ToList();

        foreach (var property in properties)
        {
            if (property.Value.Type == JTokenType.Object)
            {
                // Recursivamente procesar objetos anidados
                RemoveSpecificProperties((JObject)property.Value);
            }
            else if (property.Value.Type == JTokenType.Array)
            {
                // Recursivamente procesar arrays anidados
                foreach (var item in property.Value.ToList())
                {
                    if (item.Type == JTokenType.Object)
                    {
                        RemoveSpecificProperties((JObject)item);
                    }
                    else
                    {
                        if (item.ToString().Equals("") || item.ToString().Equals("-") || item.ToString().Equals("N/A"))
                        {
                            item.Remove();
                        }
                    }
                }
            }
            else if (property.Value.ToString().Equals("") || property.Value.ToString().Equals("-") || property.Value.ToString().Equals("N/A"))
            {
                // Quitar atributos nulos
                property.Remove();
            }
        }
    }
}
