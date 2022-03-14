using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace LibraryShared
{
    [Index("Idu001", "Idu", true)]
    public class Item
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        [DisplayName("序号")]
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Text { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }

        [Column(IsPrimary = true)]
        [DisplayName("序号U")]
        public Guid Idu { get; set; }

        public override string ToString() => $"[{Id}] {Text} ({Description})";

        public static List<Item> GenerateDatas()
        {
            var r = new Random();

            var ItemList = new List<Item>()
            {
                new Item {  Text = "假装 First item" , Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item {  Text = "的哥 Second item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item { Text = "四风 Third item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item {  Text = "加州 Fourth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item { Text = "阳光 Fifth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new Item {  Text = "孔雀 Sixth item - "+ r.Next(11000).ToString(), Description="This is an item description." ,Idu=Guid.NewGuid()}
            };

            return ItemList;
        }
    }

    [Table(DisableSyncStructure = true, Name = "Item")]
    public class ItemDto : Item
    {
        new public int? Id { get; set; }

    }
}