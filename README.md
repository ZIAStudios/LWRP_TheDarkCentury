# TheDarkCentury
RTS with no materials

- PAUTAS DE LA CREACIÓN DE PREFABS
  - Todos los prefabs tanto del mapa como las unidades tienen sus diferentes layers (Ground, Player, Enemy, Building, Sub-Building)
  - Solamente cambiar el layer del objeto padre, todos los hijos tiene que tener layer DEFAULT
  - Poner tags en edificios, hay 3 diferentes para en caso de que haya diferentes del mismo.

- Edificios y sub edificios
    - Quitar todos los mesh y poner box colliders (en el padre)

- ZONA 
  -  Padre (crear un cuadrado (tiene que tener un box collider trigger) y expandirlo hasta el tamaño en XZ deseado |Zone|)(layer - Default, poner debajo del mapa creado)

- Bandera 
  -  Padre (Box collider, |Flag| ) (layer - Flag)
    - Model (modelo de la bandera)
- EDIFICIOS 
  -  Padre (Box collider trigger, |Building|) (layer - Building)
    - Spawnpoint (ponerlo donde se quieran hacer reaparecer a las unidades)
    - SpawnArea (añadir a este hijo un box collider del tamaño óptimo (es a donde se van a mover las unidades tras spawnear)
    - Model (dentro del este hijo meter el modelo del edificio, quitar los MESH COLLIDERS)
    - Poner tag 
    
- SUB EDIFICIOS 
  -  Padre (Box colldier trigger, |Building_boost| ) (layer - Sub Building)
    - PointToMove ( Componente box collider con el tamaño para que detecte a un aldeano entrar, trigger |Building_boostPoint|)
    - Feedback (objeto que se activa y desactiva dependiendo de si hay un aldeano en el punto o no)
    - Poner tags

- PERSONAJES 
  - Padre (componenetes |Unit_Base|(esto lo tienen todos) |Unit_Melee Unit_Range|(si son tropa)) (layer - Player)
    - Model (modelo del personaje)
    - Selector ( |Selectable| )
      - Un objeto Quad de color (para dar feedback de cuando está o no seleccionado)
