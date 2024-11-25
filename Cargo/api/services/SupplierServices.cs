public class SupplierServices
{
    private SupplierAccess _supplierAccess;
    private bool _debug;
    private List<Supplier> _testSuppliers;

    public SupplierServices(SupplierAccess supplierAccess, bool debug)
    {
        _supplierAccess = supplierAccess;
        _debug = debug;
        _testSuppliers = [];
    }
    public async Task<List<Supplier>> GetSuppliers()
    {
        if (!_debug) return await _supplierAccess.GetAll();
        return _testSuppliers;
    }

    public async Task<Supplier?> GetSupplier(int supplierId)
    {
        if (!_debug) return await _supplierAccess.GetById(supplierId);
        return _testSuppliers.FirstOrDefault(s => s.Id == supplierId);
    }

    public async Task<bool> AddSupplier(Supplier supplier)
    {
        if (supplier is null || supplier.Code == "" || supplier.Name == "" || supplier.Address == "" || supplier.AddressExtra == "" || supplier.City == "" || supplier.ZipCode == "" || supplier.Province == "" || supplier.Country == "" || supplier.ContactName == "" || supplier.Phonenumber == "" || supplier.Reference == "") return false;
        List<Supplier> suppliers = await GetSuppliers();
        Supplier doubleSupplier = suppliers.FirstOrDefault(s => s.Address == supplier.Address && s.AddressExtra == supplier.AddressExtra && s.City == supplier.City && s.Code == supplier.Code && s.ContactName == supplier.ContactName && s.Country == supplier.Country && s.Name == supplier.Name && s.Phonenumber == supplier.Phonenumber && s.Province == supplier.Province && s.Reference == supplier.Reference && s.ZipCode == supplier.ZipCode)!;
        if (doubleSupplier is not null) return false;
        if (!_debug) return await _supplierAccess.Add(supplier);
        _testSuppliers.Add(supplier);
        return true;
    }

    public async Task<bool> UpdateSupplier(Supplier supplier)
    {
        if (supplier is null || supplier.Id == 0) return false;
        supplier.UpdatedAt = DateTime.Now;
        if (!_debug) return await _supplierAccess.Update(supplier);
        int foundSupplierIndex = _testSuppliers.FindIndex(s => s.Id == supplier.Id);
        if (foundSupplierIndex == -1) return false;
        _testSuppliers[foundSupplierIndex] = supplier;
        return true;
    }

    public async Task<bool> RemoveSupplier(int supplierId)
    {
        if (!_debug) return await _supplierAccess.Remove(supplierId);
        return _testSuppliers.Remove(_testSuppliers.FirstOrDefault(s => s.Id == supplierId)!);
    }
}