# Spatial-Partitioning-SimpleDeliveryGame
Spatial Grid Partition kullanarak yakın çevresine teslimat yapan bir Nav Agent içeren bir repo'dur. Spatial Partition sayesinde, şehrin içerisindeki binaların düzeni ve konumları GameDesigner tarafından değiştirilse bile sistem çalışmaya devam eder.<br><br>

<b>---SPATIAL PARTITION---</b><br>
<b>Unit.cs</b> => Grid içerisindeki her bir birimi temsil eden sınıftır. InitUnit metodu ile Unit'in sınırları içerisindeki binaları hücreler eşleştirir. Bu sayede Grid içerisindeki tüm binalar ilgili hücrelere eklenir. InitPlayerUnit metodu ise InitUnit metodu ile temelde aynı işi yapar ancak bunu sadece Player'ın GameObject'i için gerçekleştirir. <br>
<b>Grid.cs</b> => Partition işlerini yapacağımız Grid sınıfı. <b>AddBuilding</b> metodu ile verilen Unit'i ilgili hücreye ekler. Bunu yaparken custom linked list kullanılır. <b>AddPlayer</b> metodu AddBuilding ile aynı işlemi sadece Player için yapar. <b>CheckPlayerMovement</b> metodu player'ın pozisyonuna göre farklı bir hücre sınırlarına girdiyse, grid içerisindeki yerini değiştirir. <b>GetNearbyBuildings</b> metodu, verilen bir position'ın ilgili hücresini bulur, ardından bu hücrenin bitişik olduğu hücrelerdeki tüm binaları bir liste halinde döndürür. <b>ConvertFromWorldToCell</b> metodu, verilen World position'ın hangi cell'e denk geldiğini bulur.<br><br>

<b>---GAME---</b><br>
<b>BuildingFinder.cs</b> => <b>FindSameBuildingsByType</b> metodu ile verilen bir bina listesindeki, istenen tipteki binaları filtreler ve saklar. FindClosestBuilding metodu ile Chain edilerek kullanılır. <b>FindClosestBuilding</b> metodu ise NavMesh Agent'ın en az mesafede ulaşabileceği binayı bulmak için kullanılır.<br>
<b>DeliveryDestinationManager.cs</b> => Teslimat başlangıç ve bitiş binalarını hazırlayan sınıftır.<br>
<b>GameManager.cs</b> => Player'ı teslimata başlatır. Tüm binaların bir listesi burada mevcuttur.<br><br>

<b>---MAP---</b><br>
<b>IBuilding.cs</b> => Binaları soyutladığımız interface. FireStation.cs, Hospital.cs gibi bina tipleri bu interface implemente ederler.<br>
<b>DeliveryDestinationData.cs</b> => Bu bir struct'tır. Teslimat başlangıç ve bitiş binalarını içeren bir datadan ibarettir.<br>
<b>GameMapController.cs</b> => Grid'in oluşturulduğu ve yüklemesinin yapıldığı sınıftır. Ayrıca playerController'a dependencylerini yükler.<br>

<b>---PLAYER---</b><br>
<b>PlayerController.cs</b> => <b>SetNewDelivery</b> metodu ile bir teslimat rotası oluşturur. Rota, tüm grid'i aramak yerine önce yakındaki binalar üzerinden hesaplanır, böylece performans kazancı sağlanır. <b>StartToDeliver</b> metodu player'ı teslimata başlatır. <b>SetDestination</b> metodu player'ı bir binanın entry point'ine gönderir. Bir binaya ulaşıldığını collider'lar üzerinden tespit ediyoruz. <b>OnTriggerEnter</b> üzerinden hedefe ulaşıldı mı kontrolü yapılır. Ardından Player, Start Route'a varmış ise End Route'a gönderilir, End Route'a varmışsa, yeni bir teslimat oluşturulur ve süreç başa dönmüş olur. Prototipin şuanki mevcut durumunda, Player NavMeshAgent'ı sonsuz döngü içerisinde rastgele belirlenen bina tiplerine en az mesafeyi katederek teslimat yapmaktadır.
