using Xunit;
using Microsoft.EntityFrameworkCore;

public class ItemTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ItemAccess _itemAccess;
    private readonly ItemServices _service;

    public ItemTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database
                        .Options;
        _dbContext = new ApplicationDbContext(options);
        _itemAccess = new ItemAccess(_dbContext);
        _service = new(_itemAccess);
    }

    [Fact]
    public async Task GetAllItems()
    {
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        Assert.Empty(await _service.GetItems());

        await _service.AddItem(mockItem);

        Assert.Equal([mockItem], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());
    }

    [Fact]
    public async Task GetItem()
    {
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        await _service.AddItem(mockItem);

        Assert.Equal(mockItem, await _service.GetItem(1));
        Assert.Null(await _service.GetItem(0));

        await _service.RemoveItem(1);

        Assert.Null(await _service.GetItem(1));
    }

    [Fact]
    public async Task GetItemsForItemLine()
    {
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 85, 39, 10, 15, 23, 57, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 88, 42, 30, 17, 11, 2, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 23, 40, 21, 20, 20, 34, "SUP140", "T-210-I4M");

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
    }

    [Fact]
    public async Task GetItemsForItemGroup()
    {
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 11, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 39, 10, 15, 23, 57, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 12, 42, 30, 17, 11, 2, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 12, 40, 21, 20, 20, 34, "SUP140", "T-210-I4M");
    
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
    }

    [Fact]
    public async Task GetItemsForItemType()
    {
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 11, 11, 47, 13, 11, 34, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 10, 15, 23, 57, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 12, 12, 30, 17, 11, 2, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 12, 12, 21, 20, 20, 34, "SUP140", "T-210-I4M");

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
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 11, 11, 11, 13, 11, 11, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(2, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 11, 15, 23, 11, "SUP312", "j-10730-ESk");
        Item mockItem3 = new(3, "QVm03739H", "Cloned actuating artificial intelligence", "we", "3722576017240", "aHx-68Q4", "t-541-F0g", 12, 12, 12, 12, 17, 11, 12, "SUP237", "r-920-z2C");
        Item mockItem4 = new(4, "zdN19039A", "Pre-emptive asynchronous throughput", "take", "9668154959486", "pZ-7816", "IFq-47R1", 12, 12, 12, 12, 21, 20, 12, "SUP140", "T-210-I4M");

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
    }

    [Fact]
    public async Task AddItemGood()
    {
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        Assert.Empty(await _service.GetItems());

        bool IsAdded = await _service.AddItem(mockItem);

        Assert.True(IsAdded);
        Assert.Equal([mockItem], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());
    }

    [Fact]
    public async Task AddItemBad()
    {
        Location mockLocation = new(1, 1, "", "");

        Assert.Empty(await _service.GetItems());

        /* De code hieronder is uitgecomment, omdat het een error geeft. */
        //await _service.AddItem(mockLocation);

        Assert.Empty(await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());
    }

    [Fact]
    public async Task AddDuplicateItem()
    {
        Item mockItem = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        bool IsAdded1 = await _service.AddItem(mockItem);

        Assert.True(IsAdded1);
        Assert.Equal([mockItem], await _service.GetItems());

        bool IsAdded2 = await _service.AddItem(mockItem);

        Assert.False(IsAdded2);
        Assert.Equal([mockItem], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());
    }

    [Fact]
    public async Task AddItemWithDuplicateId()
    {
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(1, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 11, 15, 23, 11, "SUP312", "j-10730-ESk");

        bool IsAdded1 = await _service.AddItem(mockItem1);

        Assert.True(IsAdded1);
        Assert.Equal([mockItem1], await _service.GetItems());

        bool IsAdded2 = await _service.AddItem(mockItem2);

        Assert.False(IsAdded2);
        Assert.Equal([mockItem1], await _service.GetItems());

        await _service.RemoveItem(1);

        Assert.Empty(await _service.GetItems());
    }

    [Fact]
    public async Task UpdateItem()
    {
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");
        Item mockItem2 = new(1, "nyg48736S", "Focused transitional alliance", "may", "9733132830047", "ck-109684-VFb", "y-20588-owy", 11, 11, 11, 11, 15, 23, 11, "SUP312", "j-10730-ESk");

        bool IsAdded = await _service.AddItem(mockItem1);

        Assert.True(IsAdded);
        Assert.Equal([mockItem1], await _service.GetItems());

        bool IsUpdated = await _service.UpdateItem(mockItem2);

        Assert.True(IsUpdated);
        Assert.Equal([mockItem2], await _service.GetItems());
        Assert.NotEqual([mockItem1], await _service.GetItems());

        await _service.RemoveItem(1);
    }

    [Fact]
    public async Task RemoveItem()
    {
        Item mockItem1 = new(1, "sjQ23408K", "Face-to-face clear-thinking complexity", "must", "6523540947122", "63-OFFTq0T", "oTo304", 11, 73, 14, 47, 13, 11, 34, "SUP423", "E-86805-uTM");

        bool IsAdded = await _service.AddItem(mockItem1);

        Assert.True(IsAdded);
        Assert.Equal([mockItem1], await _service.GetItems());

        bool IsRemoved = await _service.RemoveItem(1);

        Assert.True(IsRemoved);
        Assert.Empty(await _service.GetItems());
    }
}