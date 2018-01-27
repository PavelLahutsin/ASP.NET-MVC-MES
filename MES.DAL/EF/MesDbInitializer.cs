using System;
using System.Collections.Generic;
using System.Data.Entity;
using MES.DAL.Entities;
using MES.DAL.Enums;


namespace MES.DAL.EF
{
    public class MesDbInitializer : DropCreateDatabaseIfModelChanges<MesContext>
    {
        protected override void Seed(MesContext db)
        {
            //Група деталей
            var g1 = new GroupProduct {Name = "JMT"};
            db.GroupProducts.Add(g1);

            //Детали
            var s1 = new Detail { Name = "Крышка под пружинку", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1};
            var s2 = new Detail { Name = "Крышка корпуса", VendorCode = "245-003", Quantityq = 1000, GroupProduct = g1 };
            var s3 = new Detail { Name = "Крышка №", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };
            var s4 = new Detail { Name = "Корпус 5200-01", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };
            var s5 = new Detail { Name = "Диск Верхний", VendorCode = "245-004", Quantityq = 7000, GroupProduct = g1 };
            var s6 = new Detail { Name = "Диск Нижний", VendorCode = "245-004", Quantityq = 7000, GroupProduct = g1 };
            var s7 = new Detail { Name = "Сетка", VendorCode = "245-004", Quantityq = 7000, GroupProduct = g1 };
            var s8 = new Detail { Name = "Шайба О", VendorCode = "245-004", Quantityq = 7000, GroupProduct = g1 };
            var s9 = new Detail { Name = "Шайба Н", VendorCode = "245-004", Quantityq = 7000, GroupProduct = g1 };
            var s10 = new Detail { Name = "Резинка", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };
            var s11 = new Detail { Name = "Бонка", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };
            var s12 = new Detail { Name = "Болт М8", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };
            var s13 = new Detail { Name = "Корпус 5200", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };
            var s14 = new Detail { Name = "Корпус 5200-03", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };
            var s15 = new Detail { Name = "Корпус 6500", VendorCode = "245-004", Quantityq = 1000, GroupProduct = g1 };


            db.Details.Add(s1);
            db.Details.Add(s2);
            db.Details.Add(s3);
            db.Details.Add(s4);
            db.Details.Add(s13);
            db.Details.Add(s14);
            db.Details.Add(s15);
            db.Details.Add(s8);
            db.Details.Add(s9);
            db.Details.Add(s10);
            db.Details.Add(s11);
            db.Details.Add(s12);
            db.Details.Add(s5);
            db.Details.Add(s6);
            db.Details.Add(s7);

            var p = new Product { Name = "5200-01" };
            var p2 = new Product { Name = "5200" };
            var p3 = new Product { Name = "6500" };

            db.Products.Add(p);
            db.Products.Add(p2);
            db.Products.Add(p3);

            var list = new List<StructureOfTheProduct>
            { 
            new StructureOfTheProduct { Product = p, Detail = s1, Quantity = 1 },
            new StructureOfTheProduct { Product = p, Detail = s2, Quantity = 1 },
            new StructureOfTheProduct { Product = p, Detail = s3, Quantity = 1 },
            new StructureOfTheProduct { Product = p, Detail = s4, Quantity = 1 },
            new StructureOfTheProduct { Product = p, Detail = s5, Quantity = 6 },
            new StructureOfTheProduct { Product = p, Detail = s6, Quantity = 6 },
            new StructureOfTheProduct { Product = p, Detail = s7, Quantity = 6 },
            new StructureOfTheProduct { Product = p, Detail = s8, Quantity = 7 },
            new StructureOfTheProduct { Product = p, Detail = s9, Quantity = 6 },
            new StructureOfTheProduct { Product = p, Detail = s10, Quantity = 1 },

            new StructureOfTheProduct { Product = p2, Detail = s1, Quantity = 1 },
            new StructureOfTheProduct { Product = p2, Detail = s2, Quantity = 1 },
            new StructureOfTheProduct { Product = p2, Detail = s3, Quantity = 1 },
            new StructureOfTheProduct { Product = p2, Detail = s13, Quantity = 1 },
            new StructureOfTheProduct { Product = p2, Detail = s5, Quantity = 10 },
            new StructureOfTheProduct { Product = p2, Detail = s6, Quantity = 10 },
            new StructureOfTheProduct { Product = p2, Detail = s7, Quantity = 10 },
            new StructureOfTheProduct { Product = p2, Detail = s8, Quantity = 11 },
            new StructureOfTheProduct { Product = p2, Detail = s9, Quantity = 10 },
             new StructureOfTheProduct { Product = p2, Detail = s10, Quantity = 1 },
             new StructureOfTheProduct { Product = p2, Detail = s11, Quantity = 1 },
             new StructureOfTheProduct { Product = p2, Detail = s12, Quantity = 1 },

             new StructureOfTheProduct { Product = p3, Detail = s1, Quantity = 1 },
             new StructureOfTheProduct { Product = p3, Detail = s2, Quantity = 1 },
             new StructureOfTheProduct { Product = p3, Detail = s3, Quantity = 1 },
             new StructureOfTheProduct { Product = p3, Detail = s15, Quantity = 1 },
             new StructureOfTheProduct { Product = p3, Detail = s5, Quantity = 12 },
             new StructureOfTheProduct { Product = p3, Detail = s6, Quantity = 12 },
             new StructureOfTheProduct { Product = p3, Detail = s7, Quantity = 12 },
             new StructureOfTheProduct { Product = p3, Detail = s8, Quantity = 13 },
             new StructureOfTheProduct { Product = p3, Detail = s9, Quantity = 12 },
             new StructureOfTheProduct { Product = p3, Detail = s10, Quantity = 1 },
             new StructureOfTheProduct { Product = p3, Detail = s11, Quantity = 1 },
             new StructureOfTheProduct { Product = p3, Detail = s12, Quantity = 1 }
            };

            db.StructureOfTheProducts.AddRange(list);


            db.Roles.Add(new Role { Id = 1, Name = "admin" });
            db.Roles.Add(new Role { Id = 2, Name = "user" });
            db.Users.Add(new User
            {
                UserName = "Дима",
                Password = "123456",
                RoleId = 1
            });
            db.Users.Add(new User
            {
                UserName = "Влад",
                Password = "123456",
                RoleId = 2
            });


           

            foreach (VariantStateProduct vaStPr in Enum.GetValues(typeof(VariantStateProduct)))
            {
                db.ProductStates.Add(new ProductState { Product = p, StateProduct = vaStPr, Quantity = 0});
                db.ProductStates.Add(new ProductState { Product = p2, StateProduct = vaStPr, Quantity =0 });
                db.ProductStates.Add(new ProductState { Product = p3, StateProduct = vaStPr, Quantity =0 });
            }
            
            db.SaveChanges();
        }
    }
   
}