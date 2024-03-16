using LSMTreeExample.API.Business.Interfaces;
using LSMTreeExample.API.Business.Models;

namespace LSMTreeExample.API.Business.Services
{
    public class LSMTreeService : ILSMTreeService
    {
        private List<KeyValue> memTable; // Bellekteki tablo
        private List<List<KeyValue>> sstables; // SSTable'lar // Normalde diskte olur ancak burda memeory'de tutuyoruz
        private int maxMemTableSize = 30;
        private int maxSSTableSize = 30;

        public LSMTreeService(int maxMemTableSize, int maxSSTableSize)
        {
            this.maxMemTableSize = maxMemTableSize;
            this.maxSSTableSize = maxSSTableSize;
            memTable = new List<KeyValue>();
            sstables = new List<List<KeyValue>>();
        }

        // Veri ekleme işlemi
        public void Put(int key, string value)
        {
            // Bellek tablosunda varsa güncelle, yoksa ekle
            bool found = false;
            foreach (var kvp in memTable)
            {
                if (kvp.Key == key)
                {
                    kvp.Value = value;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                memTable.Add(new KeyValue { Key = key, Value = value });
            }

            if (memTable.Count >= maxMemTableSize)
            {
                FlushMemTable();
            }
        }

        // Bellek tablosunu SSTable'a aktarma işlemi
        private void FlushMemTable()
        {
            memTable.Sort((x, y) => x.Key.CompareTo(y.Key)); // Bellek tablosunu sırala
            sstables.Add(memTable.ToList()); // SSTable'a ekle
            memTable.Clear(); // Bellek tablosunu temizle
            if (sstables.Count > 1)
            {
                Compact(); // Eğer birden fazla SSTable varsa compaction yap
            }
        }

        // Veri getirme işlemi
        public string Get(int key)
        {
            // Bellek tablosunda ara
            foreach (var kvp in memTable)
            {
                if (kvp.Key == key)
                {
                    return kvp.Value;
                }
            }

            // Bellek tablosunda bulunamazsa SSTable'ları kontrol et
            for (int i = sstables.Count - 1; i >= 0; i--)
            {
                var sstable = sstables[i];
                foreach (var kvp in sstable)
                {
                    if (kvp.Key == key)
                    {
                        return kvp.Value;
                    }
                }
            }

            return null; // Key bulunamadı
        }

        // Veri silme işlemi
        public void Delete(int key)
        {
            // Bellek tablosunda ara ve varsa null olarak işaretle
            for (int i = 0; i < memTable.Count; i++)
            {
                if (memTable[i].Key == key)
                {
                    memTable[i].Value = null;
                    return;
                }
            }

            // SSTable'larda ara ve varsa null olarak işaretle
            foreach (var sstable in sstables)
            {
                for (int i = 0; i < sstable.Count; i++)
                {
                    if (sstable[i].Key == key)
                    {
                        sstable[i].Value = null;
                        return;
                    }
                }
            }
        }

        // Compaction işlemi
        private void Compact()
        {
            var newSSTable = new List<KeyValue>();
            foreach (var sstable in sstables)
            {
                newSSTable.AddRange(sstable);
            }
            newSSTable.Sort((x, y) => x.Key.CompareTo(y.Key)); // Yeni SSTable'ı sırala
            sstables.Clear();
            sstables.Add(newSSTable.Take(maxSSTableSize).ToList()); // İlk maxSSTableSize kadarını al
        }
    }
}
