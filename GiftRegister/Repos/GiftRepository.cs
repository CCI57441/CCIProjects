using GiftRegister.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftRegister.Repos
{
    public class GiftRepository : IGiftRepository
    {
        EntertainmentDbEntities _ctx;

        public GiftRepository(EntertainmentDbEntities ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<GiftEntertainment> GetAllGifts()
        {
            return _ctx.GiftEntertainments;
        }

        public GiftEntertainment GetGift(int id)
        {
            return _ctx.GiftEntertainments.Find(id);
        }

        public bool Save()
        {
            try
            {
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                //TODO log this error
                return false;
            }
        }

        public bool AddGift(GiftEntertainment newGift)
        {
            try
            {
                _ctx.GiftEntertainments.Add(newGift);

                return true;
            }
            catch (Exception ex)
            {
                //TODO log this error
                return false;
            }
        }
    }
}