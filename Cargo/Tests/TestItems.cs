using Xunit;
using Microsoft.EntityFrameworkCore;

public class ItemTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _service;
    private readonly OrderItemMovementAccess _orderItemMovementAccess;
    private readonly ShipmentItemMovementAccess _shipmentItemMovementAccess;
    private readonly TransferItemMovementAccess _TransferItemMovementAccess;
    private readonly ItemGroupAccess _itemGroupAccess;
    private readonly ItemLineAccess _itemLineAccess;
    private readonly ItemTypeAccess _itemTypeAccess;
    private readonly ItemGroupServices _serviceItemGroup;
    private readonly ItemLineServices _serviceItemLine;
    private readonly ItemTypeServices _serviceItemType;
    private readonly SupplierAccess _supplierAccess;
    private readonly SupplierServices _serviceSupplier;

    public ItemTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new(options);
        _orderItemMovementAccess = new(_dbContext);
        _shipmentItemMovementAccess = new(_dbContext);
        _TransferItemMovementAccess = new(_dbContext);
        _itemAccess = new(_dbContext);
        _supplierAccess = new(_dbContext);
        _serviceSupplier = new(_supplierAccess, _itemAccess);
        _itemGroupAccess = new(_dbContext);
        _itemLineAccess = new(_dbContext);
        _itemTypeAccess = new(_dbContext);
        _serviceItemGroup = new(_itemGroupAccess, _itemAccess);
        _serviceItemLine = new(_itemLineAccess, _itemAccess);
        _serviceItemType = new(_itemTypeAccess, _itemAccess);
        _service = new(_itemAccess, _orderItemMovementAccess, _TransferItemMovementAccess, _shipmentItemMovementAccess, _itemGroupAccess, _itemLineAccess, _itemTypeAccess, _supplierAccess);
    }

    [Fact]
    public async Task GetAllItems()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        Assert.Empty(await _service.GetItems());

        await _service.AddItem(mockItem);

        Assert.Equal([mockItem], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task GetItem()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        await _service.AddItem(mockItem);

        Assert.Equal(mockItem, await _service.GetItem(1));
        Assert.Null(await _service.GetItem(0));

        await _service.RemoveItem(1);

        Assert.Null(await _service.GetItem(1));

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task GetItemsForItemLine()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine1 = new(11, "blablabla", "");
        ItemLine testItemLine2 = new(12, " blablablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 73, 14, 10, 15, 23, 1, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 73, 14, 30, 17, 11, 1, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 73, 14, 21, 20, 20, 1, "SUP140", "T-210-I4M");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemType.AddItemType(testItemType);

        await _service.AddItem(mockItem1);
        await _service.AddItem(mockItem2);

        Assert.Equal([mockItem1, mockItem2], await _service.GetItems());
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForItemLine(11));
        Assert.Empty(await _service.GetItemsForItemLine(10));
        Assert.Empty(await _service.GetItemsForItemLine(12));

        await _service.AddItem(mockItem3);
        await _service.AddItem(mockItem4);

        Assert.Equal([mockItem1, mockItem2, mockItem3, mockItem4], await _service.GetItems());
        Assert.Equal([mockItem3, mockItem4], await _service.GetItemsForItemLine(12));
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForItemLine(11));
        Assert.Empty(await _service.GetItemsForItemLine(13));

        await _service.RemoveItem(1);
        await _service.RemoveItem(2);
        await _service.RemoveItem(3);
        await _service.RemoveItem(4);

        Assert.Empty(await _service.GetItems());

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemLine.RemoveItemLine(12);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task GetItemsForItemGroup()
    {
        ItemGroup testItemGroup1 = new(11, "Furniture", "");
        ItemGroup testItemGroup2 = new(12, "pc", "");
        ItemLine testItemLine1 = new(11, "blablabla", "");
        ItemLine testItemLine2 = new(12, " blablablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 11, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 14, 10, 15, 23, 1, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 12, 14, 30, 17, 11, 1, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 12, 14, 21, 20, 20, 1, "SUP140", "T-210-I4M");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemType.AddItemType(testItemType);

        await _service.AddItem(mockItem1);
        await _service.AddItem(mockItem2);

        Assert.Equal([mockItem1, mockItem2], await _service.GetItems());
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForItemGroup(11));
        Assert.Empty(await _service.GetItemsForItemGroup(10));
        Assert.Empty(await _service.GetItemsForItemGroup(12));

        await _service.AddItem(mockItem3);
        await _service.AddItem(mockItem4);

        Assert.Equal([mockItem1, mockItem2, mockItem3, mockItem4], await _service.GetItems());
        Assert.Equal([mockItem3, mockItem4], await _service.GetItemsForItemGroup(12));
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForItemGroup(11));
        Assert.Empty(await _service.GetItemsForItemGroup(13));

        await _service.RemoveItem(1);
        await _service.RemoveItem(2);
        await _service.RemoveItem(3);
        await _service.RemoveItem(4);

        Assert.Empty(await _service.GetItems());

        await _serviceItemGroup.RemoveItemGroup(11);
        await _serviceItemGroup.RemoveItemGroup(12);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemLine.RemoveItemLine(12);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task GetItemsForItemType()
    {
        ItemGroup testItemGroup1 = new(11, "Furniture", "");
        ItemLine testItemLine1 = new(11, "blablabla", "");
        ItemType testItemType1 = new(11, "blablablabla", "");
        ItemGroup testItemGroup2 = new(12, "pc", "");
        ItemLine testItemLine2 = new(12, "blablablabla", "");
        ItemType testItemType2 = new(12, "blablablablabla", "");
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 11, 11, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 10, 15, 23, 1, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 12, 12, 30, 17, 11, 1, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 12, 12, 21, 20, 20, 1, "SUP140", "T-210-I4M");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);

        await _service.AddItem(mockItem1);
        await _service.AddItem(mockItem2);

        Assert.Equal([mockItem1, mockItem2], await _service.GetItems());
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForItemType(11));
        Assert.Empty(await _service.GetItemsForItemType(10));
        Assert.Empty(await _service.GetItemsForItemType(12));

        await _service.AddItem(mockItem3);
        await _service.AddItem(mockItem4);

        Assert.Equal([mockItem1, mockItem2, mockItem3, mockItem4], await _service.GetItems());
        Assert.Equal([mockItem3, mockItem4], await _service.GetItemsForItemType(12));
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForItemType(11));
        Assert.Empty(await _service.GetItemsForItemType(13));

        await _service.RemoveItem(1);
        await _service.RemoveItem(2);
        await _service.RemoveItem(3);
        await _service.RemoveItem(4);

        Assert.Empty(await _service.GetItems());
    }

    [Fact]
    public async Task GetItemsForSupplier()
    {
        ItemGroup testItemGroup1 = new(11, "Furniture", "");
        ItemLine testItemLine1 = new(11, "blablabla", "");
        ItemType testItemType1 = new(11, "blablablabla", "");
        ItemGroup testItemGroup2 = new(12, "pc", "");
        ItemLine testItemLine2 = new(12, "blablablabla", "");
        ItemType testItemType2 = new(12, "blablablablabla", "");
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 11, 11, 11, 13, 11, 11, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 11, 15, 23, 11, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 12, 12, 12, 17, 11, 12, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 12, 12, 12, 21, 20, 12, "SUP140", "T-210-I4M");
        Supplier mockSupplier1 = new(11, "SUP0001", "Lee1, Parks and Johnson1", "5989 Sullivan Drives1", "Apt. 9961", "Port Anitaburgh1", "916881", "Illinois1", "Czech Republic1", "Toni Barnett1", "363.541.7282x368251", "LPaJ-SUP0001");
        Supplier mockSupplier2 = new(12, "SUP0002", "Lee2, Parks and Johnson2", "5989 Sullivan Drives2", "Apt. 9962", "Port Anitaburgh2", "916882", "Illinois2", "Czech Republic2", "Toni Barnett2", "363.541.7282x368252", "LPaJ-SUP0002");

        await _serviceSupplier.AddSupplier(mockSupplier1);
        await _serviceSupplier.AddSupplier(mockSupplier2);
        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine1);
        await _serviceItemLine.AddItemLine(testItemLine2);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);

        await _service.AddItem(mockItem1);
        await _service.AddItem(mockItem2);

        Assert.Equal([mockItem1, mockItem2], await _service.GetItems());
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForSupplier(11));
        Assert.Empty(await _service.GetItemsForSupplier(10));
        Assert.Empty(await _service.GetItemsForSupplier(12));

        await _service.AddItem(mockItem3);
        await _service.AddItem(mockItem4);

        Assert.Equal([mockItem1, mockItem2, mockItem3, mockItem4], await _service.GetItems());
        Assert.Equal([mockItem3, mockItem4], await _service.GetItemsForSupplier(12));
        Assert.Equal([mockItem1, mockItem2], await _service.GetItemsForSupplier(11));
        Assert.Empty(await _service.GetItemsForSupplier(13));

        await _service.RemoveItem(1);
        await _service.RemoveItem(2);
        await _service.RemoveItem(3);
        await _service.RemoveItem(4);

        Assert.Empty(await _service.GetItems());

        await _serviceItemGroup.RemoveItemGroup(11);
        await _serviceItemGroup.RemoveItemGroup(12);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemLine.RemoveItemLine(12);
        await _serviceItemType.RemoveItemType(11);
        await _serviceItemType.RemoveItemType(12);
    }

    [Fact]
    public async Task AddItemGood()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        Assert.Equal([testItemType], await _serviceItemType.GetItemTypes());
        Assert.Empty(await _service.GetItems());

        bool IsAdded = await _service.AddItem(mockItem);

        Assert.True(IsAdded);
        Assert.Equal([mockItem], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());
        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task AddDuplicateItem()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);
        await _service.AddItem(mockItem);
        await _service.AddItem(mockItem);

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }

    [Fact]
    public async Task AddItemWithDuplicateId()
    {
        ItemGroup testItemGroup1 = new(73, "Furniture", "");
        ItemGroup testItemGroup2 = new(11, "pc", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType1 = new(14, "blablablabla", "");
        ItemType testItemType2 = new(11, "blabla", "");
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(1, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 11, 15, 23, 1, "SUP312", "j-10730-ESk");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);

        bool IsAdded1 = await _service.AddItem(mockItem1);

        Assert.True(IsAdded1);
        Assert.Equal([mockItem1], await _service.GetItems());

        bool IsAdded2 = await _service.AddItem(mockItem2);

        Assert.False(IsAdded2);
        Assert.Equal([mockItem1], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemGroup.RemoveItemGroup(11);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
        await _serviceItemType.RemoveItemType(11);
    }

    [Fact]
    public async Task UpdateItem()
    {
        ItemGroup testItemGroup1 = new(73, "Furniture", "");
        ItemGroup testItemGroup2 = new(11, "pc", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType1 = new(14, "blablablabla", "");
        ItemType testItemType2 = new(11, "blabla", "");
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(1, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 11, 15, 23, 1, "SUP312", "j-10730-ESk");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup1);
        await _serviceItemGroup.AddItemGroup(testItemGroup2);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType1);
        await _serviceItemType.AddItemType(testItemType2);
        await _service.AddItem(mockItem1);

        bool IsUpdated = await _service.UpdateItem(mockItem2);

        Assert.True(IsUpdated);
        Assert.Equal([mockItem2], await _service.GetItems());
        Assert.NotEqual([mockItem1], await _service.GetItems());

        await _service.RemoveItem(1);

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemGroup.RemoveItemGroup(11);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
        await _serviceItemType.RemoveItemType(11);
    }

    [Fact]
    public async Task RemoveItem()
    {
        ItemGroup testItemGroup = new(73, "Furniture", "");
        ItemLine testItemLine = new(11, "blablabla", "");
        ItemType testItemType = new(14, "blablablabla", "");
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 1, "SUP423", "E-86805-uTM");
        Supplier mockSupplier = new(1, "SUP0001", "Lee, Parks and Johnson", "5989 Sullivan Drives", "Apt. 996", "Port Anitaburgh", "91688", "Illinois", "Czech Republic", "Toni Barnett", "363.541.7282x36825", "LPaJ-SUP0001");

        await _serviceSupplier.AddSupplier(mockSupplier);
        await _serviceItemGroup.AddItemGroup(testItemGroup);
        await _serviceItemLine.AddItemLine(testItemLine);
        await _serviceItemType.AddItemType(testItemType);

        bool IsAdded = await _service.AddItem(mockItem1);

        Assert.True(IsAdded);
        Assert.Equal([mockItem1], await _service.GetItems());

        bool IsRemoved = await _service.RemoveItem(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItems());

        await _serviceItemGroup.RemoveItemGroup(73);
        await _serviceItemLine.RemoveItemLine(11);
        await _serviceItemType.RemoveItemType(14);
    }
}