Transform schema and operations to dart types with anotations for simple_json

Currently supports
- Schema Types
- enums
- Operation types
- Selection Types
- Operation Vraible types

example result:

```dart
import 'package:simple_json_mapper/simple_json_mapper.dart';

@JsonObject()
class Query {
  late DocumentsConnection? documents;
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
class DocumentsConnection {
  late PageInfo pageInfo;
  late List<DocumentsEdge>? edges;
  late List<Document>? nodes;

  @JProp(ignore: true)
  final String __typename = "DocumentsConnection";
}

@JsonObject()
class PageInfo {
  late bool hasNextPage;
  late bool hasPreviousPage;
  late String? startCursor;
  late String? endCursor;

  @JProp(ignore: true)
  final String __typename = "PageInfo";
}

@JsonObject()
class Document {
  late String title;
  late String id;
  late DateTime created;

  @JProp(ignore: true)
  final String __typename = "Document";
}

@JsonObject()
class DocumentsEdge {
  late String cursor;
  late Document node;

  @JProp(ignore: true)
  final String __typename = "DocumentsEdge";
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
  late DocumentsQueryDocumentsConnection? documents;

  @JProp(ignore: true)
  final String __typename = "DocumentsQuery";
}

class DocumentsQueryArgs {
  late String? before;
  late String? after;
  late int? first;
  late int? last;
}

@JsonObject()
class DocumentsQueryDocumentsConnection {
  late List<DocumentsQueryDocument>? nodes;
  late DocumentsQueryPageInfo info;

  @JProp(ignore: true)
  final String __typename = "DocumentsQueryDocumentsConnection";
}

@JsonObject()
class DocumentsQueryDocument {
  late String id;

  @JProp(ignore: true)
  final String __typename = "DocumentsQueryDocument";
}

@JsonObject()
class DocumentsQueryPageInfo {
  late bool hasNextPage;

  @JProp(ignore: true)
  final String __typename = "DocumentsQueryPageInfo";
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
