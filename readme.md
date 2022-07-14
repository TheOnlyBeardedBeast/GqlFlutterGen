Transform schema and operations to dart types with anotations for simple_json

Currently supports
- Schema Types
- enums
- Operation types
- Selection Types

example result:

tested type maps within a real application in flutter

```dart
import 'package:simple_json_mapper/simple_json_mapper.dart';
@JsonObject()
class Query {
    final List<Document> documents;
    final List<Marker> markers;
    final List<Notification> notifications;
    final List<Photo> photos;

    const Query({ required this.documents,required this.markers,required this.notifications,required this.photos, });

    @JProp(ignore: true)
    final String __typename = "Query";
}


@JsonObject()
class Mutation {
    final int? seed;

    const Mutation({ required this.seed, });

    @JProp(ignore: true)
    final String __typename = "Mutation";
}


@JsonObject()
class Photo {
    final String? caption;
    final String folder;
    final String path;
    final String id;
    final DateTime created;

    const Photo({ required this.caption,required this.folder,required this.path,required this.id,required this.created, });

    @JProp(ignore: true)
    final String __typename = "Photo";
}


@JsonObject()
class Notification {
    final String title;
    final String content;
    final String id;
    final DateTime created;

    const Notification({ required this.title,required this.content,required this.id,required this.created, });

    @JProp(ignore: true)
    final String __typename = "Notification";
}


@JsonObject()
class Marker {
    final String? name;
    final int? number;
    final MarkerType type;
    final String id;
    final DateTime created;

    const Marker({ required this.name,required this.number,required this.type,required this.id,required this.created, });

    @JProp(ignore: true)
    final String __typename = "Marker";
}


@JsonObject()
class Document {
    final String title;
    final String id;
    final DateTime created;

    const Document({ required this.title,required this.id,required this.created, });

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
    final List<DocumentsQueryDocument> documents;

    const DocumentsQuery({ required this.documents, });

    @JProp(ignore: true)
    final String __typename = "DocumentsQuery";
}


@JsonObject()
class DocumentsQueryDocument {
    final String id;
    final String title;
    final DateTime created;

    const DocumentsQueryDocument({ required this.id,required this.title,required this.created, });

    @JProp(ignore: true)
    final String __typename = "DocumentsQueryDocument";
}


@JsonObject()
class NotificationsQuery {
    final List<NotificationsQueryNotification> notifications;

    const NotificationsQuery({ required this.notifications, });

    @JProp(ignore: true)
    final String __typename = "NotificationsQuery";
}


@JsonObject()
class NotificationsQueryNotification {
    final String id;
    final String title;
    final DateTime created;
    final String content;

    const NotificationsQueryNotification({ required this.id,required this.title,required this.created,required this.content, });

    @JProp(ignore: true)
    final String __typename = "NotificationsQueryNotification";
}


@JsonObject()
class MarkersQuery {
    final List<MarkersQueryMarker> markers;

    const MarkersQuery({ required this.markers, });

    @JProp(ignore: true)
    final String __typename = "MarkersQuery";
}


@JsonObject()
class MarkersQueryMarker {
    final String id;
    final MarkerType type;
    final DateTime created;

    const MarkersQueryMarker({ required this.id,required this.type,required this.created, });

    @JProp(ignore: true)
    final String __typename = "MarkersQueryMarker";
}


@JsonObject()
class SeedMutation {
    final int? seed;

    const SeedMutation({ required this.seed, });

    @JProp(ignore: true)
    final String __typename = "SeedMutation";
}
```
