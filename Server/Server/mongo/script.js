conn = new Mongo();
db = conn.getDB("message");
db.createCollection('Message');
db.Message.insertMany([
	{'ChannelId':1,'UserId':1,'Content':'This is the first message in final fantasy','Time':'2019-01-01 00:00:00.0010000'},
	{'ChannelId':1,'UserId':2,'Content':'And this is the second message in final fantasy','Time':'2019-01-02 00:00:01.2450000'},
	{'ChannelId':1,'UserId':3,'Content':'AAAAAAAAAA','Time':'2019-01-02 00:00:02.3680000'},
	{'ChannelId':2,'UserId':1,'Content':'Another channel in final fantasy','Time':'2019-01-02 00:00:01.1230000'},
	{'ChannelId':2,'UserId':1,'Content':'BBBBBBBBBBBBBB','Time':'2019-01-02 00:00:02.8990000'},
	{'ChannelId':2,'UserId':2,'Content':'Hi there','Time':'2019-01-02 00:00:03.5430000'}
]);