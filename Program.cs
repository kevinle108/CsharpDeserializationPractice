using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = Pokemon("lugia");
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
            Pokemon lugia = JsonSerializer.Deserialize<Pokemon>(data, options);
            Console.WriteLine(lugia.Id);
            Console.WriteLine(lugia.Height);
            foreach (var item in lugia.Abilities)
            {
                Console.WriteLine(item.Ability.Name);
            }
        }

        static string GetDataFromHttp(string url)
        {
            // client
            // requestMessage
            // responseMessage
            // stream
            // streamReader
            // stringData

            using HttpClient client = new();
            HttpRequestMessage request = new(HttpMethod.Get, url);
            HttpResponseMessage response = client.Send(request);
            Stream stream = response.Content.ReadAsStream();
            using StreamReader reader = new(stream);
            string data = reader.ReadToEnd();
            return data;
        }

        static string Pokemon(string pokeName)
        {
            HttpClient client = new();
            HttpRequestMessage webRequest = new(HttpMethod.Get, "https://pokeapi.co/api/v2/pokemon/" + pokeName);
            HttpResponseMessage response = client.Send(webRequest);
            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }

        static void PokemonAbilities()
        {
            string data = Pokemon("lugia");
            using JsonDocument doc = JsonDocument.Parse(data);
            JsonElement root = doc.RootElement;
            string pokemonName = root.GetProperty("species").GetProperty("name").GetString();
            JsonElement abilities = root.GetProperty("abilities");
            for (int i = 0; i < abilities.GetArrayLength(); i++)
            {
                Console.WriteLine($"{pokemonName} has the ability {abilities[i].GetProperty("ability").GetProperty("")}");
            }
        }

        static void PullAPart()
        {
            string data = GetDataFromHttp("https://inventoryservice.pullapart.com/Make/");
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
            CarMake[] makes = JsonSerializer.Deserialize<CarMake[]>(data, options);
            int rareCount = 0;
            int nonRareCount = 0;
            foreach (CarMake make in makes)
            {
                if (make.RareFind)
                {
                    rareCount++;
                    //Console.WriteLine(make.MakeName);
                }
                if (!make.RareFind)
                {
                    nonRareCount++;
                    //Console.WriteLine(make.MakeName);
                }
            }
            Console.WriteLine($"There are {makes.Length} makes.");
            Console.WriteLine($"Rares: {rareCount}");
            Console.WriteLine($"NonRares: {nonRareCount}");
        }

    }

    class CarMake
    {
        public int MakeID { get; set; }
        public string MakeName { get; set; }
        public bool RareFind { get; set; }
    }

    class Pokemon
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public AbilityItem[] Abilities { get; set; }
    }

    class AbilityItem
    {
        public class AbilityObject
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public AbilityObject Ability { get; set; }
    }
}
