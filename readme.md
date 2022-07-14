Transform schema and operations to dart types with anotations for simple_json

Currently supports
- Schema Types
- enums
- Operation types
- Selection Types
- Operation Vraible types

TODO: 
- Add constructors

example result:

```dart
import 'package:simple_json_mapper/simple_json_mapper.dart';

@JsonObject()
class Query {
  late List<Document> documents;
  late List<Marker> markers;
  late List<Notification> notifications;
  late List<Photo> photos;

  @JProp(ignore: true)
  final String __typename = "Query";
}

@JsonObject()
class Mutation {
  late int? seed;

  @JProp(ignore: true)
  final String __typename = "Mutation";
}

@JsonObject()
class Photo {
  late String? caption;
  late String folder;
  late String path;
  late String id;
  late DateTime created;

  @JProp(ignore: true)
  final String __typename = "Photo";
}

@JsonObject()
class Notification {
  late String title;
  late String content;
  late String id;
  late DateTime created;

  @JProp(ignore: true)
  final String __typename = "Notification";
}

@JsonObject()
class Marker {
  late String? name;
  late int? number;
  late MarkerType type;
  late String id;
  late DateTime created;

  @JProp(ignore: true)
  final String __typename = "Marker";
}

@JsonObject()
class Document {
  late String title;
  late String id;
  late DateTime created;

  @JProp(ignore: true)
  final String __typename = "Document";
}

enum MarkerType {
  @EnumValue(value: "WATCHTOWER")
  // ignore: constant_identifier_names
  WATCHTOWER,
  @EnumValue(value: "SALTBASE")
  // ignore: constant_identifier_names
  SALTBASE,
  @EnumValue(value: "FEEDER")
  // ignore: constant_identifier_names
  FEEDER,
}

@JsonObject()
class DocumentsQuery {
  List<DocumentsQueryDocument>? documents;

  @JProp(ignore: true)
  final String __typename = "DocumentsQuery";
}

@JsonObject()
class DocumentsQueryDocument {
  late String id;
  late String title;
  late DateTime created;

  @JProp(ignore: true)
  final String __typename = "DocumentsQueryDocument";
}

@JsonObject()
class NotificationsQuery {
  late List<NotificationsQueryNotification> notifications;

  @JProp(ignore: true)
  final String __typename = "NotificationsQuery";
}

@JsonObject()
class NotificationsQueryNotification {
  late String id;
  late String title;
  late DateTime created;
  late String content;

  @JProp(ignore: true)
  final String __typename = "NotificationsQueryNotification";
}

@JsonObject()
class MarkersQuery {
  late List<MarkersQueryMarker> markers;

  @JProp(ignore: true)
  final String __typename = "MarkersQuery";
}

@JsonObject()
class MarkersQueryMarker {
  late String id;
  late MarkerType type;
  late DateTime created;

  @JProp(ignore: true)
  final String __typename = "MarkersQueryMarker";
}

@JsonObject()
class SeedMutation {
  late int? seed;

  @JProp(ignore: true)
  final String __typename = "SeedMutation";
}
```
