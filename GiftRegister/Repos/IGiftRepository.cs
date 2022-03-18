using GiftRegister.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftRegister.Repos
{
    public interface IGiftRepository
    {
        IQueryable<GiftEntertainment> GetAllGifts();
        GiftEntertainment GetGift(int id);

        bool Save();
        bool AddGift(GiftEntertainment newGift);

    }
}
