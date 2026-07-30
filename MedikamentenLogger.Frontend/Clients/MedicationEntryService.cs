using MedikamentenLogger.Frontend.Models;
using MedikamentenLogger.Shared.Dtos.EntryDtos;

namespace MedikamentenLogger.Frontend.Clients;

public class MedicationEntryService(HttpClient httpClient)
{

    public async Task<PageEntryDto[]> GetMedicationEntriesByMedicationId(int id)
    {
        return await httpClient.GetFromJsonAsync<PageEntryDto[]>($"/entries/pageEntries/{id}") ?? [];
    }

    public async Task<OpenedEntryDto> GetMedicationEntryByID(int id)
    {
        return (await httpClient.GetFromJsonAsync<OpenedEntryDto>($"/entries/openedEntry/{id}"))!;
    }

    public async Task<EntryDetailsDto> GetEntryDetailsDto(int id)
    {
        return (await httpClient.GetFromJsonAsync<EntryDetailsDto>($"/entries/entryDetails/{id}"))!;
    }

    public async Task<EntryDetailsDto?> AddMedicationEntry(CreateEntryDto entry)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"/entries", entry);
        response.EnsureSuccessStatusCode();


        if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<EntryDetailsDto>();
        else return null;
    }

    public async Task<bool> DeleteMedicationEntryById(int id)
    {
        using HttpResponseMessage response = await httpClient.DeleteAsync($"/entries/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateMedicationEntry(int id, UpdateEntryDto entry)
    {
        HttpResponseMessage response = await httpClient.PutAsJsonAsync($"/entries/{id}", entry);
        return response.IsSuccessStatusCode;
    }
}