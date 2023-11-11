using System.Net;

namespace rqdq.aoc22;

public class InputRetriever {
  
  private readonly HttpClient _client;
  private static readonly Uri AocDomain = new("https://adventofcode.com");

  public InputRetriever(string token) {
    var cookieContainer = new CookieContainer();
    cookieContainer.Add(AocDomain, new Cookie("session", token));
    _client = new HttpClient(new HttpClientHandler()
    {
      AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
      CookieContainer = cookieContainer
    });
    _client.BaseAddress = AocDomain; }

  public
  string Fetch(DateTime dt) {
    var response = _client.GetAsync($@"{dt.Year}/day/{dt.Day}/input").Result;
    response.EnsureSuccessStatusCode();
    return response.Content.ReadAsStringAsync().Result; } }