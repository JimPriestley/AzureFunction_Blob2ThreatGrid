using RestSharp;

public static void Run(Stream myBlob, string name, TraceWriter log, out string outputQueueItem)
{
    var client = new RestClient("https://<threatgrid uri>/api/v2/");

    var request = new RestRequest("samples", Method.POST);
    request.AlwaysMultipartFormData = true;

    // add parameters for all properties on an object
    request.AddObject(new Sample(name, myBlob));
    // add parameter "private"
    request.AddParameter("private", "true");
    
    // execute the request
    IRestResponse response = client.Execute(request);
    outputQueueItem = response.Content; // raw content as string
    log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes\n{response.Content}");
 }

public class Sample
{
    public byte[] sample { get; set; }
    public string filename { get; set; }
    public string os { get; set; }
    public string osver { get; set; }
    public string vm { get; set; }
    public string privates { get; set; }
    public string source { get; set; }
    public string api_key { get; set; }
    public string tags { get; set; }

    public Sample(string Filename, Stream myBlob)
    {
        this.filename = Filename;
        this.os = "Unknown";
        this.osver = "Unknown";
        this.vm = "";
        this.privates = "true";
        this.tags = "";
        using (MemoryStream ms = new MemoryStream())
        {
            myBlob.CopyTo(ms);
            this.sample = ms.ToArray();
        }
    }
}
