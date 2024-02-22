# Spatial-Partitioning-SimpleDeliveryGame
Spatial Grid Partition kullanarak yakın çevresine teslimat yapan bir Nav Agent içeren bir repo'dur.<br><br>

<b>---SPATIAL PARTITION---</b><br>
<b>Unit.cs</b> => Grid içerisindeki her bir birimi temsil eden sınıftır. InitUnit metodu ile Unit'in sınırları içerisindeki binaları hücreler eşleştirir. Bu sayede Grid içerisindeki tüm binalar ilgili hücrelere eklenir. InitPlayerUnit metodu ise InitUnit metodu ile temelde aynı işi yapar ancak bunu sadece Player'ın GameObject'i için gerçekleştirir. <br>
<b>Grid.cs</b> => Partition işlerini yapacağımız Grid sınıfı. <b>AddBuilding</b> metodu ile verilen Unit'i ilgili hücreye ekler. Bunu yaparken custom linked list kullanılır. <b>AddPlayer</b> metodu AddBuilding ile aynı işlemi sadece Player için yapar. <b>CheckPlayerMovement</b> metodu player'ın pozisyonuna göre farklı bir hücre sınırlarına girdiyse, grid içerisindeki yerini değiştirir. <b>GetNearbyBuildings</b> metodu, verilen bir position'ın ilgili hücresini bulur, ardından bu hücrenin bitişik olduğu hücrelerdeki tüm binaları bir liste halinde döndürür. <b>ConvertFromWorldToCell</b> metodu, verilen World position'ın hangi cell'e denk geldiğini bulur.<br><br>

<b>---GAME---</b><br>
<b>BuildingFinder.cs</b> => <b>FindSameBuildingsByType</b> metodu ile verilen bir bina listesindeki, istenen tipteki binaları filtreler ve saklar. FindClosestBuilding metodu ile Chain edilerek kullanılır. <b>FindClosestBuilding</b> metodu ise NavMesh Agent'ın en az mesafede ulaşabileceği binayı bulmak için kullanılır.<br>

