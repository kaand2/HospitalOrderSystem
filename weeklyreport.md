**1. Hafta**



İlk haftamda dotnet kullanımını ve temel bilgileri öğrendim. Solutionımın klasör ve dosya yapısını clean architecture yapımıza göre düzenledim ve Domain, Application, Persistence, Infrastructure, API ve Tests projelerine ayırdım. Solution içindeki projelerin referanslarını ekledim ve görevlerini anlamaya çalıştım. Controller, service ve repository yapılarını oluşturup temel görevlerini öğrendim. Entitylerimi ve enumlarımı oluşturdum.



Daha sonra Entity Framework Core ve Microsoft SQL Server entegrasyonunu öğrendim. ProjectDbContext sınıfını oluşturarak patient entitysini veritabanı tablolarına dönüştürülmesini sağladım. İlk migrationımı aldıktan sonra ilk SQL databaseimi oluşturdum.



Entity’ler arasındaki ilişkileri planlarken foreign key, unique index gibi konular üzerinde çalıştım. İlk haftanın ana modülü olarak Patient modülünü geliştirdim. Hasta oluşturma, listeleme, ID ile getirme, güncelleme ve silme işlemleri için DTO, repository, service ve controller yapılarını oluşturdum.



Veri doğrulama işlemleri için FluentValidation kullandım. TC kimlik numarasının 11 haneli olması, zorunlu alanların boş bırakılmaması, doğum tarihinin gelecekte olmaması (ilk başta entitynin içinde de kontrolünü yapmıştım) ve e-posta formatının kontrol edilmesi gibi kuralları validator sınıflarında tanımladım.



Entity ve DTO arasındaki dönüşümler için AutoMapper kullandım. Aynı TC kimlik numarasıyla birden fazla hasta oluşturulmasını ve databasede karışıklığı engellemek için veritabanında TcNo için unique constraint kullandım.



Hasta silme işleminde ise kaydı tamamen silmek yerine soft delete ile hastaları verilerini silmeden silinmiş durumuna aldım. Haftanın sonunda Patient modülünün tüm CRUD işlemlerini tamamladım ve oluşturduğum endpoint’leri Swagger üzerinden manuel olarak test ettim.



**2. Hafta**



Stajımın ikinci haftasında ilk hafta oluşturduğum temel yapının üzerine projenin diğer ana modüllerini geliştirdim. Bu hafta User, Auth, Order ve OrderAction modülleri üzerinde çalıştım. Entitylerim arttıkça ve birbirlerine bağlandıkça navigation property ve cascade davranışları gibi konulara çalıştım.



İlk olarak kullanıcı yönetimi için User DTO’ları, repository ve service yapılarını oluşturdum. Sayıları arttıkça bunları birbirinden ayırmanın projeyi ne kadar daha okunaklı ve büyük ölçeklerde ne kadar daha control edilebilir kıldığını gördüm.



Kullanıcı şifrelerinin veritabanında açık şekilde tutulmaması için IPasswordHasher<User> kullanarak şifreleri hashledim. Kullanıcının sisteme giriş yapabilmesi için login işlemini geliştirdim ve başarılı giriş sonrasında JWT oluşturulmasını sağladım. JWT içerisine kullanıcı ID’si, kullanıcı adı ve rol bilgilerini claim olarak ekledim.



Sistemde başlangıçta kullanıcı bulunmadığı için ilk Admin kullanıcısının oluşturulabileceği bir bootstrap-admin endpoint’i hazırladım. Daha sonra JWT Bearer Authentication ve role-based authorization yapılarını projeye ekleyip yetkisi olmayan kullanıcıların işlem yapmasını engelledim.



Öncelikle elle bir JWT key yazıp bunu bearer token olarak kullanarak login yaparak imzalı bir JWT almasam da her şeyi halledebileceğimi düşündüm. Ama hata aldığım için sistemde başlangıçta anonym olarak ilk Admin kullanıcısının oluşturulabileceği bir bootstrap-admin endpoint’i hazırladım. Bu hesap ile login yaparak imzalı doğru bir token alabilmeyi sağladım.



Böylece Admin, Doctor ve Nurse gibi kullanıcı rollerinin hangi endpoint’lere erişebileceğini kontrol edebildim ve her adımda yetkisiz erişimi engelledim. Swagger’a Bearer token desteği ekleyerek login sonrasında aldığım JWT ile authorization gerektiren endpoinleri de swagger üzerinden test ettim.



Bu süreçte authentication ile authorization arasındaki farkı, JWT’yi, key oluşturmayı ve \[Authorize] attribute’unun çalışma mantığını daha iyi öğrendim.



İkinci hafta ayrıca Order ve OrderAction modüllerini oluşturdum. Order entity’sini Patient ve User ile ilişkilendirdim ve order oluşturma, güncelleme, silme ve iptal işlemlerini yazdım.



Order durumlarının rastgele ileri geri değiştirilememesi için OrderService içerisinde belli orderstatus geçişleri oluşturdum. Updatelenen orderactionlardan eski order durumuna geçilmemesi gibi kuralları service içerisinde kontrol ettim.



Order iptal işlemi için ayrı bir endpoint ve CancelOrderDto oluşturarak iptal nedeninin zorunlu olmasını sağladım.



Son olarak OrderAction modülünü geliştirerek order işlemlerinin ayrı kayıtlar halinde tutulabileceği bir table yapısı oluşturdum. OrderAction endpoint’i üzerinden önceki durum, yeni durum, işlem türü, açıklama, sonuç ve işlem zamanı kaydedilebiliyor.



Ayrıca hata yönetimini tek bir yerde toplamak amacıyla ExceptionMiddleware oluşturup Middlewarei uygulamalı olarak öğrendim. Böylece hataları HTTP status kodlarına dönüştürerek controllerlar içerisinde sürekli tekrar eden try-catch bloklarının önüne geçtim.



İkinci haftanın sonunda kullanıcı yönetimi, JWT authentication, rol bazlı yetkilendirme, order yönetimi, order durum geçişleri, iptal işlemleri ve order action geçmişi gibi projenin ana işlevlerini tamamladım.

