public class SupplierServices
{
    public async Task<List<Supplier>> GetSuppliers() => await AccessJson.ReadJson<Supplier>();

    public async Task<Supplier> GetSupplier(Guid supplierId)
    {
        List<Supplier> suppliers = await GetSuppliers();
        return suppliers.FirstOrDefault(s => s.Id == supplierId)!;
    }

    public async Task<bool> AddSupplier(Supplier supplier)
    {
        if (supplier is null || supplier.Code == "" || supplier.Name == "" || supplier.Address == "" || supplier.AddressExtra == "" || supplier.City == "" || supplier.ZipCode == "" || supplier.Province == "" || supplier.Country == "" || supplier.ContactName == "" || supplier.Phonenumber == "" || supplier.Reference == "") return false;
        
        List<Supplier> suppliers = await GetSuppliers();
        Supplier doubleSupplier = suppliers.FirstOrDefault(s => s.Address == supplier.Address && s.AddressExtra == supplier.AddressExtra && s.City == supplier.City && s.Code == supplier.Code && s.ContactName == supplier.ContactName && s.Country == supplier.Country && s.Name == supplier.Name && s.Phonenumber == supplier.Phonenumber && s.Province == supplier.Province && s.Reference == supplier.Reference && s.ZipCode == supplier.ZipCode)!;
        if (doubleSupplier is not null) return false;

        supplier.Id = Guid.NewGuid();
        await AccessJson.WriteJson(supplier);
        return true;
    }

    public async Task<bool> UpdateSupplier(Supplier supplier)
    {
        if (supplier is null || supplier.Id == Guid.Empty) return false;

        List<Supplier> suppliers = await GetSuppliers();
        int foundSupplierIndex = suppliers.FindIndex(s => s.Id == supplier.Id);
        if (foundSupplierIndex == -1) return false;

        supplier.UpdatedAt = DateTime.Now;
        suppliers[foundSupplierIndex] = supplier;
        AccessJson.WriteJsonList(suppliers);
        return true;
    }

    public async Task<bool> RemoveSupplier(Guid supplierId)
    {
        if (supplierId == Guid.Empty) return false;

        List<Supplier> suppliers = await GetSuppliers();
        Supplier foundSupplier = suppliers.FirstOrDefault(s => s.Id == supplierId)!;
        if (foundSupplier is null) return false;

        suppliers.Remove(foundSupplier);
        AccessJson.WriteJsonList(suppliers);
        return true;
    }
}