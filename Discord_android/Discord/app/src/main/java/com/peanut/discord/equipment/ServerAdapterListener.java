package com.peanut.discord.equipment;

import com.peanut.discord.models.Server;

interface ServerAdapterListener {
    void onSelectServer(Server server);
}
