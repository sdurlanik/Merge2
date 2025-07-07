# Merge2 Oyunu: Proje Mimarisi Dokümantasyonu 🎮

Bu doküman, **Merge2** projesinin yazılım mimarisini ve temel tasarım prensiplerini detaylandırmaktadır.  
Temel amaç, modern tasarım prensipleri ve desenlerine bağlı kalarak **sağlam, ölçeklenebilir ve bakımı kolay** bir kod tabanı oluşturmaktır.

---

## ✨ 1. Temel Felsefeler

Mimarimiz üç ana felsefe üzerine kurulmuştur:

### 🧩 Veri Odaklı Tasarım (Data-Driven Design)
- Oyun verileri ve ayarları, koddan bağımsız **ScriptableObject** asset'leri ile yönetilir.  
- Bu, tasarımcıların kod değiştirmeden oyun dengesini kolayca ayarlamasına olanak tanır.

### 🔄 Bağımsız İletişim (Decoupled Communication)
- Sistemler arası iletişim, merkezi bir **Event Bus** üzerinden sağlanır.  
- Bu, birbirine doğrudan bağımlı olmayan, **modüler ve esnek sistemler** yaratır.

### 🧱 Sorumlulukların Ayrılması (Separation of Concerns)
- Proje, **Veri (Model), Mantık (Service)** ve **Görünüm (View)** olmak üzere katmanlı bir mimari kullanır.  
- **S.O.L.I.D.** prensiplerini takip eder.

---

## 🏛️ 2. Temel Mimari Desenler

Bu desenler, projenin merkezi sinir sistemini oluşturur ve sistemlerin birbiriyle uyum içinde çalışmasını sağlar.

### 2.1. Service Locator Deseni
- Klasik Singleton deseninin yarattığı sıkı bağımlılık sorunlarından kaçınmak için merkezi bir **Service Locator** kullanıyoruz.

#### Yapı:
- `ServiceLocator.cs`: `GridManager`, `OrderManager` gibi tüm ana yönetici nesnelerini kaydeden statik bir sınıf.
- Diğer sınıflar `ServiceLocator.Get<T>()` çağırarak servislere erişir.

#### Faydaları:
- Bağımlılıkları netleştirir.
- Test kolaylığı sağlar.
- `GameManager`, tüm servisleri kaydederek uygulamanın başlatılma dizisini merkezileştirir.

---

### 2.2. Event Bus (Observer Deseni)
- Sistemler arası iletişim için birincil aracımızdır ve mimarinin en önemli parçalarından biridir.

#### Yapı:
- `EventBus<T>.cs`: Olayları yayınlamak ve bu olaylara abone olmak için kullanılan, **type-safe** statik sınıf.
- `IEvent`: Tüm olay veri yapıları (struct) için işaretçi arayüz.

#### Faydaları:
- Yayıncıları ve aboneleri tamamen birbirinden ayırır.
- Örnek: `OrderUIManager`, `OrderManager`'dan habersiz şekilde sipariş güncellemelerini dinler.  
  > Bu, **Bağımlılıkların Tersine Çevrilmesi Prensibi**’nin mükemmel bir uygulamasıdır.

---

## 🏗️ 3. Sistem Analizi (Katmanlı Mimari)

Proje, üç belirgin kavramsal katmana ayrılmıştır:

### 3.1. 💾 Veri ve Model Katmanı
- Tüm oyun yapılandırması ve çalışma zamanı durumu bu katmanda tutulur.
- **Oyun mantığı veya MonoBehaviour içermez.**

#### Yapılandırma (ScriptableObject):
- `ItemSO`, `GeneratorSO`, `ProductSO`: Tüm item'ların statik özelliklerini tanımlar.
- `OrderDataSO`: Farklı sipariş türleri için şablonlar içerir.
- `SettingsSO`: `LevelDesignSettingsSO`, `AnimationSettingsSO` gibi ayar varlıkları.

#### Çalışma Zamanı (Plain C# Classes):
- `PlayerData.cs`: Para, grid durumu gibi verileri tutan, serileştirilebilir sınıf.
- `Order.cs`: Oyun sırasında aktif olan siparişi temsil eder.

---

### 3.2. ⚙️ Servis ve Mantık Katmanı
- Oyunun kurallarını ve mantığını içerir.
- **Model katmanını değiştirir ve View'dan tamamen bağımsızdır.**

#### Yöneticiler (Stateful Services):
- `GameManager`, `GridManager`, `OrderManager`, `DataManager`, `ObjectPooler`

#### Statik Servisler (Stateless Logic):
- `SaveLoadService`, `ItemActionService`, `OrderGeneratorService`, `BoardSetupService`

---

### 3.3. 🖼️ Görünüm ve Sunum Katmanı
- Oyuncunun gördüğü ve etkileşime girdiği tüm unsurlar bu katmanda yer alır.

#### MonoBehaviour Bileşenleri:
- `ItemInteractionManager`: Girdi olaylarını yakalar, EventBus olayına çevirir.
- `AnimationManager`: Tüm DOTween animasyonlarını merkezi olarak yönetir.
- `Cell` & `Item`: Grid üzerindeki görsel temsil.

#### UI Bileşenleri:
- `OrderUIManager`, `OrderUIEntry`, `CurrencyUI` gibi bileşenler EventBus üzerinden ekranı günceller.

---

## 🎨 4. Uygulanan Tasarım Desenleri

- **State Deseni**: `Cell` sınıfının durumlarını (`LockedHidden`, `LockedRevealed`, `Unlocked`) yönetir.
- **Chain of Responsibility**: Merge, Move, Swap gibi işlemleri zincir şeklinde işler.
- **Factory Deseni**: `ItemFactory.cs`, item üretimini merkezileştirir.
- **Object Pool Deseni**: `ObjectPooler.cs`, nesneleri geri dönüştürerek performansı artırır.

---

## 💎 5. Pratikte S.O.L.I.D. Prensipleri

| İlke | Açıklama |
|------|----------|
| **S (Tek Sorumluluk)** | Her sınıfın yalnızca bir sorumluluğu vardır. Örn: `SaveLoadService` sadece kayıt/yükleme yapar. |
| **O (Açık/Kapalı)** | Yeni özellikler mevcut kodu değiştirmeden eklenebilir. Örn: Yeni Handler eklemek. |
| **L (Liskov Yerine Geçme)** | `GeneratorSO`, `ItemSO` yerine kullanılabilir. |
| **I (Arayüz Ayırımı)** | `IDraggable`, `ITappable` gibi küçük ve özelleşmiş arayüzler. |
| **D (Bağımlılıkların Tersine Çevrilmesi)** | `EventBus`, UI ve Mantık katmanlarını olaylar üzerinden soyutlamalarla bağlar. |

---

> Bu mimari yapı, Merge2'nin sürdürülebilir bir şekilde gelişmesini ve genişlemesini kolaylaştırır.
