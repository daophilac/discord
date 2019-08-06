package com.peanut.discord.models;
import java.util.List;
public class Role {
    private int roleId;
    private String roleName;
    private boolean canDelete;
    private int serverId;
    private Server server;
    private List<ServerLevelPermission> serverLevelPermissions;
    private List<ChannelLevelPermission> channelLevelPermissions;
    public int getRoleId() {
        return roleId;
    }
    public void setRoleId(int roleId) {
        this.roleId = roleId;
    }
    public String getRoleName() {
        return roleName;
    }
    public void setRoleName(String roleName) {
        this.roleName = roleName;
    }
    public int getServerId() {
        return serverId;
    }
    public void setServerId(int serverId) {
        this.serverId = serverId;
    }
    public boolean isCanDelete() {
        return canDelete;
    }
    public void setCanDelete(boolean canDelete) {
        this.canDelete = canDelete;
    }
    public Server getServer() {
        return server;
    }
    public void setServer(Server server) {
        this.server = server;
    }
    public List<ServerLevelPermission> getServerLevelPermissions() {
        return serverLevelPermissions;
    }
    public void setServerLevelPermissions(List<ServerLevelPermission> serverLevelPermissions) {
        this.serverLevelPermissions = serverLevelPermissions;
    }
    public List<ChannelLevelPermission> getChannelLevelPermissions() {
        return channelLevelPermissions;
    }
    public void setChannelLevelPermissions(List<ChannelLevelPermission> channelLevelPermissions) {
        this.channelLevelPermissions = channelLevelPermissions;
    }
}
