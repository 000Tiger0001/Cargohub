public class SupplierServices
{
    private readonly SupplierAccess _supplierAccess;
    private readonly ItemAccess _itemAccess;

    public SupplierServices(SupplierAccess supplierAccess, ItemAccess itemAccess)
    {
        _supplierAccess = supplierAccess;
        _itemAccess = itemAccess;
    }

    public async Task<List<Supplier>> GetSuppliers() => await _supplierAccess.GetAll();

    public async Task<Supplier?> GetSupplier(int supplierId) => await _supplierAccess.GetById(supplierId);

    public async Task<bool> AddSupplier(Supplier supplier)
    {
        if (supplier is null || supplier.Code == "" || supplier.Name == "" || supplier.Address == "" || supplier.AddressExtra == "" || supplier.City == "" || supplier.ZipCode == "" || supplier.Province == "" || supplier.Country == "" || supplier.ContactName == "" || supplier.Phonenumber == "" || supplier.Reference == "") return false;
        List<Supplier> suppliers = await GetSuppliers();
        Supplier doubleSupplier = suppliers.FirstOrDefault(s => s.Address == supplier.Address && s.AddressExtra == supplier.AddressExtra && s.City == supplier.City && s.Code == supplier.Code && s.ContactName == supplier.ContactName && s.Country == supplier.Country && s.Name == supplier.Name && s.Phonenumber == supplier.Phonenumber && s.Province == supplier.Province && s.Reference == supplier.Reference && s.ZipCode == supplier.ZipCode)!;
        if (doubleSupplier is not null) return false;
        return await _supplierAccess.Add(supplier);
    }

    public async Task<bool> UpdateSupplier(Supplier supplier)
    {
        if (supplier is null || supplier.Id <= 0) return false;
        supplier.UpdatedAt = DateTime.Now;
        return await _supplierAccess.Update(supplier);
    }

    public async Task<bool> RemoveSupplier(int supplierId)
    {
        List<Item> items = await _itemAccess.GetAll();
        items.ForEach(i => { if (i.SupplierId == supplierId) i.SupplierId = 0; });
        await _itemAccess.UpdateMany(items);
        return await _supplierAccess.Remove(supplierId);
    }
}