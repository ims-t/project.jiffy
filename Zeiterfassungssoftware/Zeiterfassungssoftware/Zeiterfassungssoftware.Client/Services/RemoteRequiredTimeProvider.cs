using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Zeiterfassungssoftware.SharedData.RequiredTimes;
using Zeiterfassungssoftware.SharedData.Times;

namespace Zeiterfassungssoftware.Client.Services
{
    public class RemoteRequiredTimeProvider : IRequiredTimeProvider
    {
        public bool IsLoaded { get; set; }
        public List<RequiredTimeDto> _requiredTimes { get; set; }

        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        public HttpClient HttpClient { get; set; } = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7099/api/v1/requiredtimes/")
        };


        public RemoteRequiredTimeProvider() { 
            LoadRequiredTimes();
        }

        public async void LoadRequiredTimes()
        {
            _requiredTimes = await HttpClient.GetFromJsonAsync<List<RequiredTimeDto>>("") ?? new();
            IsLoaded = true;
        }

        public async Task<RequiredTimeDto> CreateRequiredTime(RequiredTimeDto requiredTime)
        {
            var Response = await HttpClient.PostAsJsonAsync("", requiredTime);

            try
            {
                Response.EnsureSuccessStatusCode();
                var ReponseContent = await Response.Content.ReadAsStringAsync();
                var ConfirmedRequiredTime = JsonSerializer.Deserialize<RequiredTimeDto>(ReponseContent, Options) ?? new();
                
                _requiredTimes.Add(ConfirmedRequiredTime);
                return ConfirmedRequiredTime;
            }
            catch (Exception e)
            {
                throw new InvalidDataException();
            }
        }

        public async Task<RequiredTimeDto> UpdateRequiredTime(Guid id, RequiredTimeDto requiredTime)
        {
            var Response = await HttpClient.PutAsJsonAsync(id.ToString(), requiredTime);

            try
            {
                Response.EnsureSuccessStatusCode();

                var Body = await Response.Content.ReadAsStringAsync();
                var ConfirmedRequiredTime = JsonSerializer.Deserialize<RequiredTimeDto>(Body, Options);

                if (ConfirmedRequiredTime is null)
                    throw new Exception();


                var RequiredTime = _requiredTimes.FirstOrDefault(e => e.Id == id);
                if (RequiredTime is null)
                {
                    _requiredTimes.Add(ConfirmedRequiredTime);
                }
                else
                {
                    var index = _requiredTimes.IndexOf(RequiredTime);
                    _requiredTimes[index] = ConfirmedRequiredTime;
                }

                return ConfirmedRequiredTime;
            }
            catch (Exception e)
            {
                if (Response.StatusCode == HttpStatusCode.NotFound)
                    throw new KeyNotFoundException();
                else
                    throw new InvalidDataException();
            }
        }

        public async Task DeleteRequiredTime(Guid id)
        {
            var Response = await HttpClient.DeleteAsync(id.ToString());

            try
            {
                Response.EnsureSuccessStatusCode();
                var RequiredTime = _requiredTimes.FirstOrDefault(e => e.Id == id);

                if (RequiredTime is not null)
                    _requiredTimes.Remove(RequiredTime);
            }
            catch (Exception e)
            {
                throw new KeyNotFoundException();
            }
        }

        public async Task<RequiredTimeDto> GetRequiredTimeById(Guid id)
        {
            var RequiredTime = await HttpClient.GetFromJsonAsync<RequiredTimeDto>(id.ToString());

            if (RequiredTime is null)
                throw new KeyNotFoundException();

            return RequiredTime;
        }

        public List<RequiredTimeDto> GetRequiredTimes()
        {
            return _requiredTimes;
        }
    }
}
