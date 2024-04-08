<h1>LibraryApp</h1>
<p>LibraryApp, mikroservis mimarisi kullanarak bir kütüphane uygulaması oluşturmayı amaçlar.</p>

<h2>Hizmetler</h2>
<p>Bu uygulama, IdentityService, LoanService ve StockService olmak üzere üç ana hizmetten oluşur:</p>

<p>IdentityService: Diğer servislerin güvenliğini sağlamak için identityserver4 kütüphanesini kullanır ve PostgreSQL veritabanını kullanır.</p>

<p>LoanService: Kütüphanenin üyelere ödünç olarak vereceği kitapları yönetir ve PostgreSQL veritabanını kullanır.</p>

<p>StockService: Kütüphanenin stoklarını yönetir ve MongoDB veritabanını kullanarak stok miktarlarını event sourcing tekniği ile takip eder.</p>

<h2>İstemci Uygulama</h2>
<p>Client uygulaması Angular kullanılarak geliştirilmiştir. İlk girişinizde, eğer giriş yapmadıysanız, otomatik olarak IdentityService kimlik doğrulama endpointine yönlendirilirsiniz. Burada hesap oluşturabilir veya giriş yapabilirsiniz. Giriş yaptıktan sonra, yetkisiz bir kullanıcı sadece stokları inceleyebilir, kitap ödünç alma talebinde bulunabilir veya önceden yapmış olduğunuz kitap ödünç alma taleplerini inceleyebilir.</p>

<p>Eğer yetkili bir kullanıcı iseniz, size özel olarak bulunan admin endpointine erişebilirsiniz. Burada mevcut stokları güncelleyebilir, ödünç alma taleplerini inceleyebilir veya ödünç alınan kitapların iadesini gerçekleştirebilirsiniz.</p>

<h2>Kullanılan Teknolojiler</h2>
<h3>API</h3>
<ul>
  <li>Identity Server 4</li>
  <li>IdentityServer4.AspNetIdentity</li>
  <li>PostgreSQL</li>
  <li>RabbitMQ</li>
  <li>Dapper</li>
  <li>Quartz</li>
  <li>MongoDB</li>
</ul>
<h3>Client</h3>
<ul>
  <li>Alertify JS</li>
  <li>NGX-Spinner</li>
  <li>angular-auth-oidc-client</li>
</ul>

<h2>Daha Fazla Bilgi</h2>
<p>Uygulama hakkında daha fazla bilgi edinmek istiyorsanız, kodları inceleyebilirsiniz veya profilimdeki iletişim bilgileri kısmından bana ulaşıp sorularınızı sorabilirsiniz.</p>
